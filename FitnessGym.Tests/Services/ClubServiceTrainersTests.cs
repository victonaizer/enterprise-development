using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Enums;

namespace FitnessGym.Tests.Services;

/// <summary>
/// Тесты CRUD-операций с тренерами и аналитических запросов по тренерам в ClubService.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ClubServiceTrainersTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
{
    /// <summary>
    /// Должен возвращать только тренеров, стаж работы которых не менее 5 лет.
    /// </summary>
    [Fact]
    public void GetExperiencedTrainersShouldReturnTrainersWithAtLeastFiveYearsExperience()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
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
        var actualTrainers = service.GetExperiencedTrainers(minYears: 5);

        // assert
        Assert.Equal(expectedTrainerNames, actualTrainers.Select(t => t.FullName));
    }

    /// <summary>
    /// Должен возвращать пять наиболее популярных тренеров, упорядоченных по убыванию популярности.
    /// </summary>
    [Fact]
    public void GetPopularTrainersShouldReturnTopFiveOrderedByPopularity()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
        string[] expectedTrainerNames =
        [
            "Волкова Ирина Дмитриевна",
            "Новиков Артур Валерьевич",
            "Семёнов Кирилл Андреевич",
            "Кузнецова Наталья Сергеевна",
            "Морозов Георгий Львович",
        ];

        // act
        var actualTrainers = service.GetPopularTrainers(count: 5);

        // assert
        Assert.Equal(expectedTrainerNames, actualTrainers.Select(t => t.FullName));
    }

    /// <summary>
    /// Должен добавлять тренера, присваивая ему следующий свободный идентификатор.
    /// </summary>
    [Fact]
    public void AddTrainerShouldAssignNextIdentifierAndStoreTrainer()
    {
        // arrange
        var service = new ClubService(fixture.Data, () => fixture.Now);
        var expectedTrainersCount = fixture.Trainers.Count + 1;
        var newTrainer = new Trainer
        {
            PassportNumber = "4699 999999",
            FullName = "Тестова Анастасия Игоревна",
            Gender = Gender.Female,
            BirthDate = new DateOnly(1995, 5, 5),
            Specialization = fixture.Specializations[0],
            ExperienceYears = 5,
        };

        // act
        var addedTrainer = service.AddTrainer(newTrainer);
        var storedTrainer = service.GetTrainer(addedTrainer.Id);

        // assert
        Assert.Equal(expectedTrainersCount, service.GetTrainers().Count);
        Assert.Equal(newTrainer.FullName, storedTrainer?.FullName);
    }
}
