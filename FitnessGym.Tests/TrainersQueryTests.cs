using FitnessGym.Domain.Entities;

namespace FitnessGym.Tests;

/// <summary>
/// Тесты запроса информации о тренерах по стажу работы.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class TrainersQueryTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать только тренеров, стаж работы которых не менее 5 лет.
    /// </summary>
    [Fact]
    public void TrainersWithAtLeastFiveYearsExperienceShouldBeReturned()
    {
        // arrange
        string[] expectedTrainerNames =
        [
            "Кузнецова Наталья Сергеевна",
            "Лебедева Дарья Александровна",
            "Новиков Артур Валерьевич",
            "Морозов Георгий Львович",
            "Волкова Ирина Дмитриевна",
            "Павлова Светлана Юрьевна",
            "Семёнов Кирилл Андреевич",
            "Голубева Вероника Игоревна",
        ];

        // act
        var actualTrainers = fixture.Trainers
            .Where(t => t.ExperienceYears >= 5)
            .ToList();

        // assert
        Assert.Equal(expectedTrainerNames, actualTrainers.Select(t => t.FullName));
    }
}
