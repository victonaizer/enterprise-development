using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Enums;

namespace FitnessGym.Tests.Services;

/// <summary>
/// Тесты CRUD-операций с клиентами и запроса клиентов с просроченным абонементом в ClubService.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ClubServiceClientsTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать только клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    [Fact]
    public void GetClientsWithExpiredSubscriptionShouldReturnClientsOrderedByFullName()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
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
        var service = new ClubService(fixture.Data, () => fixture.Now);
        var expectedClientsCount = fixture.Clients.Count + 1;
        var newClient = new Client
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
        var service = new ClubService(fixture.Data, () => fixture.Now);
        var expectedClientsCount = fixture.Clients.Count;
        var updatedClient = new Client
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
    /// Должен удалять клиента по идентификатору.
    /// </summary>
    [Fact]
    public void DeleteClientShouldRemoveClientByIdentifier()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
        var expectedClientsCount = fixture.Clients.Count - 1;

        // act
        var isDeleted = service.DeleteClient(0);

        // assert
        Assert.True(isDeleted);
        Assert.Null(service.GetClient(0));
        Assert.Equal(expectedClientsCount, service.GetClients().Count);
    }
}
