using FitnessGym.Application.Services;
using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
using FitnessGym.Domain.Enums;
using FitnessGym.Infrastructure.InMemory;

namespace FitnessGym.Tests.Services;

/// <summary>
/// Тесты CRUD-операций с клиентами и запроса клиентов с просроченным абонементом в ClubService.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ClubServiceClientsTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Создаёт сервис на репозиториях фикстуры с детерминированными часами.
    /// </summary>
    private IClubService CreateService() => new ClubService(
        new InMemoryClientRepository(fixture.Clients),
        new InMemoryTrainerRepository(fixture.Trainers),
        new InMemorySpecializationRepository(fixture.Specializations),
        new InMemoryTrainingSessionRepository(fixture.Sessions),
        () => fixture.Now);

    /// <summary>
    /// Должен возвращать только клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    [Fact]
    public void GetClientsWithExpiredSubscriptionShouldReturnClientsOrderedByFullName()
    {
        // arrange
        var service = CreateService();
        string[] expectedClientNames =
        [
            "Андреев Алексей Сергеевич",
            "Борисова Марина Владимировна",
            "Васильев Дмитрий Олегович",
            "Гаврилова Елена Николаевна",
            "Дмитриев Павел Андреевич",
        ];

        // act
        var actualClients = service.GetClientsWithExpiredSubscription();

        // assert
        Assert.Equal(expectedClientNames, actualClients.Select(c => c.FullName));
    }

    /// <summary>
    /// Должен добавлять клиента, присваивая ему следующий свободный идентификатор.
    /// </summary>
    [Fact]
    public void AddClientShouldAssignNextIdentifierAndStoreClient()
    {
        // arrange
        var service = CreateService();
        var expectedClientsCount = fixture.Clients.Count + 1;
        var newClient = new ClientDto
        {
            PassportNumber = "4599 999999",
            FullName = "Тестов Тест Тестович",
            Gender = Gender.Male,
            BirthDate = new DateOnly(2000, 1, 1),
            Phone = "+7 (911) 999-99-99",
            SubscriptionStart = fixture.Today,
            SubscriptionEnd = fixture.Today.AddDays(30),
        };

        // act
        var addedClient = service.AddClient(newClient);
        var storedClient = service.GetClient(addedClient.Id);

        // assert
        Assert.Equal(expectedClientsCount, service.GetClients().Count);
        Assert.Equal(newClient.FullName, storedClient?.FullName);
    }

    /// <summary>
    /// Должен заменять данные существующего клиента, сохраняя его идентификатор.
    /// </summary>
    [Fact]
    public void UpdateClientShouldReplaceExistingClientKeepingIdentifier()
    {
        // arrange
        var service = CreateService();
        var expectedClientsCount = fixture.Clients.Count;
        var updatedClient = new ClientDto
        {
            PassportNumber = fixture.Clients[0].PassportNumber,
            FullName = fixture.Clients[0].FullName,
            Gender = fixture.Clients[0].Gender,
            BirthDate = fixture.Clients[0].BirthDate,
            Phone = "+7 (911) 555-55-55",
            SubscriptionStart = fixture.Clients[0].SubscriptionStart,
            SubscriptionEnd = fixture.Clients[0].SubscriptionEnd,
        };

        // act
        var isUpdated = service.UpdateClient(0, updatedClient);
        var storedClient = service.GetClient(0);

        // assert
        Assert.True(isUpdated);
        Assert.Equal(expectedClientsCount, service.GetClients().Count);
        Assert.Equal("+7 (911) 555-55-55", storedClient?.Phone);
    }

    /// <summary>
    /// Должен удалять клиента, у которого нет записей на занятия.
    /// </summary>
    [Fact]
    public void DeleteClientWithoutSessionsShouldRemoveClient()
    {
        // arrange
        var service = CreateService();
        var clientWithoutSessionsId = 10;
        var expectedClientsCount = fixture.Clients.Count - 1;

        // act
        var isDeleted = service.DeleteClient(clientWithoutSessionsId);

        // assert
        Assert.True(isDeleted);
        Assert.Null(service.GetClient(clientWithoutSessionsId));
        Assert.Equal(expectedClientsCount, service.GetClients().Count);
    }

    /// <summary>
    /// Должен запрещать удалять клиента, у которого есть записи на занятия.
    /// </summary>
    [Fact]
    public void DeleteClientWithSessionsShouldThrow()
    {
        // arrange
        var service = CreateService();
        var clientWithSessionsId = fixture.Sessions[0].Client.Id;

        // act, assert
        Assert.Throws<InvalidOperationException>(() => service.DeleteClient(clientWithSessionsId));
        Assert.NotNull(service.GetClient(clientWithSessionsId));
    }
}
