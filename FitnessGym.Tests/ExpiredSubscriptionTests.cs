using FitnessGym.Domain.Entities;

namespace FitnessGym.Tests;

/// <summary>
/// Тесты запроса информации о клиентах с истёкшим абонементом.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ExpiredSubscriptionTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать только клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    [Fact]
    public void ExpiredSubscriptionClientsShouldBeReturnedOrderedByFullName()
    {
        // arrange
        var today = fixture.Today;
        string[] expectedClientNames =
        [
            "Андреев Алексей Сергеевич",
            "Борисова Марина Владимировна",
            "Васильев Дмитрий Олегович",
            "Гаврилова Елена Николаевна",
            "Дмитриев Павел Андреевич",
        ];

        // act
        var actualClients = fixture.Clients
            .Where(c => c.SubscriptionEnd < today)
            .OrderBy(c => c.FullName)
            .ToList();

        // assert
        Assert.Equal(expectedClientNames, actualClients.Select(c => c.FullName));
    }
}
