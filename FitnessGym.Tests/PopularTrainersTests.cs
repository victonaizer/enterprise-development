namespace FitnessGym.Tests;

/// <summary>
/// Тесты определения наиболее популярных тренеров.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class PopularTrainersTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать пять тренеров с наибольшим количеством записанных занятий,
    /// упорядоченных по убыванию популярности.
    /// </summary>
    [Fact]
    public void TopFivePopularTrainersShouldBeReturnedOrderedByPopularity()
    {
        // arrange
        string[] expectedTrainerNames =
        [
            "Волкова Ирина Дмитриевна",
            "Новиков Артур Валерьевич",
            "Семёнов Кирилл Андреевич",
            "Кузнецова Наталья Сергеевна",
            "Морозов Георгий Львович",
        ];

        // act
        var actualTrainerNames = fixture.Sessions
            .GroupBy(s => s.Trainer)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.FullName)
            .ToList();

        // assert
        Assert.Equal(expectedTrainerNames, actualTrainerNames);
    }
}
