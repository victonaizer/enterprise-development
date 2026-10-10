using FitnessGym.Application.Services;
using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
using FitnessGym.Domain.Enums;
using FitnessGym.Infrastructure.InMemory;

namespace FitnessGym.Tests.Services;

/// <summary>
/// Тесты CRUD-операций с тренерами и аналитических запросов по тренерам в ClubService.
/// </summary>
/// <param name="fixture">Фикстура с тестовыми данными фитнес-клуба.</param>
public class ClubServiceTrainersTests(FitnessGymFixture fixture) : IClassFixture<FitnessGymFixture>
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
    /// Должен возвращать только тренеров, стаж работы которых не менее 5 лет.
    /// </summary>
    [Fact]
    public void GetExperiencedTrainersShouldReturnTrainersWithAtLeastFiveYearsExperience()
    {
        // arrange
        var service = CreateService();
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
        var service = CreateService();
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
    /// Должен добавлять тренера с существующей специализацией,
    /// присваивая ему следующий свободный идентификатор.
    /// </summary>
    [Fact]
    public void AddTrainerShouldAssignNextIdentifierAndStoreTrainer()
    {
        // arrange
        var service = CreateService();
        var expectedTrainersCount = fixture.Trainers.Count + 1;
        var newTrainer = new TrainerDto
        {
            PassportNumber = "4699 999999",
            FullName = "Тестова Анастасия Игоревна",
            Gender = Gender.Female,
            BirthDate = new DateOnly(1995, 5, 5),
            SpecializationId = fixture.Specializations[0].Id,
            ExperienceYears = 5,
        };

        // act
        var addedTrainer = service.AddTrainer(newTrainer);
        var storedTrainer = service.GetTrainer(addedTrainer.Id);

        // assert
        Assert.Equal(expectedTrainersCount, service.GetTrainers().Count);
        Assert.Equal(newTrainer.FullName, storedTrainer?.FullName);
        Assert.Equal("Йога", storedTrainer?.SpecializationName);
    }

    /// <summary>
    /// Должен запрещать добавлять тренера с несуществующей специализацией.
    /// </summary>
    [Fact]
    public void AddTrainerWithMissingSpecializationShouldThrow()
    {
        // arrange
        var service = CreateService();
        var newTrainer = new TrainerDto
        {
            PassportNumber = "4698 888888",
            FullName = "Несуществующий Артём Львович",
            Gender = Gender.Male,
            BirthDate = new DateOnly(1990, 1, 1),
            SpecializationId = 999,
            ExperienceYears = 3,
        };

        // act, assert
        Assert.Throws<InvalidOperationException>(() => service.AddTrainer(newTrainer));
        Assert.Equal(fixture.Trainers.Count, service.GetTrainers().Count);
    }

    /// <summary>
    /// Должен запрещать удалять тренера, у которого есть записи на занятия.
    /// </summary>
    [Fact]
    public void DeleteTrainerWithSessionsShouldThrow()
    {
        // arrange
        var service = CreateService();
        var trainerWithSessionsId = fixture.Sessions[0].Trainer.Id;

        // act, assert
        Assert.Throws<InvalidOperationException>(() => service.DeleteTrainer(trainerWithSessionsId));
        Assert.NotNull(service.GetTrainer(trainerWithSessionsId));
    }
}
