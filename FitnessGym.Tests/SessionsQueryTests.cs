using FitnessGym.Domain.Entities;

namespace FitnessGym.Tests;

/// <summary>
/// Тесты запроса информации о занятиях за текущий месяц в выбранном зале.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class SessionsQueryTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать только занятия, проходящие в выбранном зале в текущем месяце,
    /// и не возвращать занятия того же зала в других месяцах и занятия других залов в текущем месяце.
    /// </summary>
    [Fact]
    public void CurrentMonthSessionsInSelectedHallShouldBeReturned()
    {
        // arrange
        var hallName = "Силовой зал";
        var today = fixture.Today;
        int[] expectedSessionIds = [0, 1, 2];

        // act
        var actualSessions = fixture.Sessions
            .Where(s => s.HallName == hallName
                        && s.StartsAt.Year == today.Year
                        && s.StartsAt.Month == today.Month)
            .ToList();

        // assert
        Assert.Equal(expectedSessionIds, actualSessions.Select(s => s.Id));
    }
}
