using FitnessGym.Application.Services;
using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
using FitnessGym.Infrastructure.InMemory;

namespace FitnessGym.Tests.Services;

/// <summary>
/// Тесты CRUD-операций с записями на занятия и аналитических запросов по занятиям и залам в ClubService.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ClubServiceSessionsTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
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
    /// Должен возвращать только занятия, проходящие в выбранном зале в текущем месяце.
    /// </summary>
    [Fact]
    public void GetSessionsForCurrentMonthShouldReturnSessionsInSelectedHall()
    {
        // arrange
        var service = CreateService();
        var hallName = "Силовой зал";
        int[] expectedSessionIds = [0, 1, 2];

        // act
        var actualSessions = service.GetSessionsForCurrentMonth(hallName);

        // assert
        Assert.Equal(expectedSessionIds, actualSessions.Select(s => s.Id));
    }

    /// <summary>
    /// Должен определить, что зал недоступен, если в нём идёт занятие, пересекающееся по времени с искомым моментом,
    /// и доступен, если занятие уже закончилось или в зале ещё не было занятий.
    /// </summary>
    /// <param name="hallName">Название проверяемого зала.</param>
    /// <param name="expectedAvailability">Ожидаемая доступность зала.</param>
    [Theory]
    [InlineData("Зал групповых занятий", false)] // в зафиксированный момент в зале идёт занятие
    [InlineData("Зал единоборств", true)] // занятие в зале закончилось 10 минут назад
    [InlineData("Кардио-зал", true)] // занятий с известным интервалом в зале ещё не было
    public void IsHallAvailableShouldBeDeterminedByOverlappingSession(string hallName, bool expectedAvailability)
    {
        // arrange
        var service = CreateService();

        // act
        var isHallAvailable = service.IsHallAvailable(hallName);

        // assert
        Assert.Equal(expectedAvailability, isHallAvailable);
    }

    /// <summary>
    /// Должен добавлять запись на занятие по существующим клиенту и тренеру,
    /// присваивая ей следующий свободный идентификатор.
    /// </summary>
    [Fact]
    public void AddSessionShouldAssignNextIdentifierAndStoreSession()
    {
        // arrange
        var service = CreateService();
        var expectedSessionsCount = fixture.Sessions.Count + 1;
        var newSession = new TrainingSessionDto
        {
            ClientId = 5,
            TrainerId = 107, // Волкова Ирина Дмитриевна
            StartsAt = fixture.Now.AddDays(1),
            HallName = "Силовой зал",
            IsTrial = false,
        };

        // act
        var addedSession = service.AddSession(newSession);
        var storedSession = service.GetSession(addedSession.Id);

        // assert
        Assert.Equal(expectedSessionsCount, service.GetSessions().Count);
        Assert.Equal(newSession.HallName, storedSession?.HallName);
        Assert.Null(storedSession?.EndsAt);
    }

    /// <summary>
    /// Должен запрещать добавлять запись на занятие с несуществующим тренером.
    /// </summary>
    [Fact]
    public void AddSessionWithMissingTrainerShouldThrow()
    {
        // arrange
        var service = CreateService();
        var newSession = new TrainingSessionDto
        {
            ClientId = 5,
            TrainerId = 999,
            StartsAt = fixture.Now.AddDays(1),
            HallName = "Силовой зал",
            IsTrial = false,
        };

        // act, assert
        Assert.Throws<InvalidOperationException>(() => service.AddSession(newSession));
        Assert.Equal(fixture.Sessions.Count, service.GetSessions().Count);
    }
}
