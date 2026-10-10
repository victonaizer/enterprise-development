namespace FitnessGym.Tests;

/// <summary>
/// Тесты проверки доступности зала для записи.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class HallAvailabilityTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен определить, что зал недоступен, если в нём идёт занятие, пересекающееся по времени с текущим моментом,
    /// и доступен, если занятие уже закончилось или в зале ещё не было занятий.
    /// Занятия с ещё не проставленным временем окончания в проверке не участвуют.
    /// </summary>
    /// <param name="hallName">Название проверяемого зала.</param>
    /// <param name="expectedAvailability">Ожидаемая доступность зала.</param>
    [Theory]
    [InlineData("Зал групповых занятий", false)] // прямо сейчас в зале идёт занятие
    [InlineData("Зал единоборств", true)] // занятие в зале закончилось 10 минут назад
    [InlineData("Кардио-зал", true)] // занятий в зале ещё не было
    public void HallAvailabilityShouldBeDeterminedByOverlappingSession(string hallName, bool expectedAvailability)
    {
        // arrange
        var now = fixture.Now;

        // act
        var isHallAvailable = !fixture.Sessions.Any(s =>
            s.HallName == hallName && s.EndsAt != null && s.StartsAt <= now && now < s.EndsAt.Value);

        // assert
        Assert.Equal(expectedAvailability, isHallAvailable);
    }
}
