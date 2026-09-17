using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;

namespace FitnessGym.Tests.Services;

/// <summary>
/// Тесты CRUD-операций с записями на занятия и аналитических запросов по занятиям и залам в ClubService.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ClubServiceSessionsTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать только занятия, проходящие в выбранном зале в текущем месяце.
    /// </summary>
    [Fact]
    public void GetSessionsForCurrentMonthShouldReturnSessionsInSelectedHall()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
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
        var service = new ClubService(fixture.Data, () => fixture.Now);

        // act
        var isHallAvailable = service.IsHallAvailable(hallName);

        // assert
        Assert.Equal(expectedAvailability, isHallAvailable);
    }

    /// <summary>
    /// Должен добавлять запись на занятие, присваивая ей следующий свободный идентификатор.
    /// </summary>
    [Fact]
    public void AddSessionShouldAssignNextIdentifierAndStoreSession()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
        var expectedSessionsCount = fixture.Sessions.Count + 1;
        var newSession = new TrainingSession
        {
            Client = fixture.Clients[5],
            Trainer = fixture.Trainers[7],
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
}
