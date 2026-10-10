using FitnessGym.Contracts.Dto;
using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Enums;
using FitnessGym.Infrastructure.PostgreSql;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitnessGym.Tests.PostgreSql;

/// <summary>
/// Тесты EF-реализаций репозиториев. В качестве провайдера используется SQLite
/// в памяти: тесты выполняются детерминированно без сервера базы данных,
/// при этом покрывают ту же логику репозиториев, что работает с PostgreSQL.
/// </summary>
public class PostgreSqlRepositoriesTests
{
    /// <summary>
    /// Создаёт контекст базы данных поверх SQLite в памяти со схемой и справочником специализаций.
    /// </summary>
    private static FitnessGymDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var context = new FitnessGymDbContext(
            new DbContextOptionsBuilder<FitnessGymDbContext>()
                .UseSqlite(connection)
                .Options);
        context.Database.EnsureCreated();
        return context;
    }

    /// <summary>
    /// Должен добавлять клиента, присваивая ему следующий свободный идентификатор.
    /// </summary>
    [Fact]
    public void AddClientShouldAssignNextIdentifier()
    {
        // arrange
        using var context = CreateContext();
        var repository = new PostgreSqlClientRepository(context);
        context.Clients.AddRange(
            new Client { Id = 0, PassportNumber = "4501 000001", FullName = "Клиент Первый", Gender = Gender.Male, BirthDate = new DateOnly(1990, 1, 1), Phone = "+7 (900) 000-00-01", SubscriptionStart = new DateOnly(2026, 1, 1), SubscriptionEnd = new DateOnly(2026, 12, 31) },
            new Client { Id = 5, PassportNumber = "4501 000002", FullName = "Клиент Шестой", Gender = Gender.Female, BirthDate = new DateOnly(1995, 5, 5), Phone = "+7 (900) 000-00-02", SubscriptionStart = new DateOnly(2026, 1, 1), SubscriptionEnd = new DateOnly(2026, 12, 31) });
        context.SaveChanges();

        var newClient = new Client
        {
            PassportNumber = "4501 000003",
            FullName = "Клиент Новый",
            Gender = Gender.Male,
            BirthDate = new DateOnly(2000, 10, 10),
            Phone = "+7 (900) 000-00-03",
            SubscriptionStart = new DateOnly(2026, 2, 1),
            SubscriptionEnd = new DateOnly(2026, 11, 30),
        };

        // act
        var addedClient = repository.Add(newClient);

        // assert
        Assert.Equal(6, addedClient.Id);
        Assert.Equal(3, repository.GetAll().Count);
    }

    /// <summary>
    /// Должен обновлять тренера, включая смену специализации.
    /// </summary>
    [Fact]
    public void UpdateTrainerShouldAllowChangingSpecialization()
    {
        // arrange
        using var context = CreateContext();
        var repository = new PostgreSqlTrainerRepository(context);
        var specialization = context.Specializations.First(s => s.Id == 5);
        var trainer = new Trainer
        {
            Id = 0,
            PassportNumber = "4601 000001",
            FullName = "Тренер Первый",
            Gender = Gender.Male,
            BirthDate = new DateOnly(1985, 3, 3),
            Specialization = context.Specializations.First(s => s.Id == 0),
            ExperienceYears = 4,
        };
        context.Trainers.Add(trainer);
        context.SaveChanges();
        trainer.Specialization = specialization;

        // act
        var isUpdated = repository.Update(0, trainer);
        var storedTrainer = repository.GetById(0);

        // assert
        Assert.True(isUpdated);
        Assert.Equal(5, storedTrainer?.Specialization.Id);
    }

    /// <summary>
    /// Должен удалять клиента по идентификатору.
    /// </summary>
    [Fact]
    public void DeleteClientShouldRemoveClient()
    {
        // arrange
        using var context = CreateContext();
        var repository = new PostgreSqlClientRepository(context);
        context.Clients.Add(
            new Client { Id = 0, PassportNumber = "4501 000001", FullName = "Клиент Первый", Gender = Gender.Male, BirthDate = new DateOnly(1990, 1, 1), Phone = "+7 (900) 000-00-01", SubscriptionStart = new DateOnly(2026, 1, 1), SubscriptionEnd = new DateOnly(2026, 12, 31) });
        context.SaveChanges();

        // act
        var isDeleted = repository.Delete(0);

        // assert
        Assert.True(isDeleted);
        Assert.Null(repository.GetById(0));
    }
}
