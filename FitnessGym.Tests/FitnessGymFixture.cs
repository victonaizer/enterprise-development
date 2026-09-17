using FitnessGym.Domain.Data;
using FitnessGym.Domain.Entities;

namespace FitnessGym.Tests;

/// <summary>
/// Фикстура, содержащая тестовые данные фитнес-клуба.
/// </summary>
public class FitnessGymFixture
{
    /// <summary>
    /// Момент времени, относительно которого построены тестовые данные.
    /// Фиксируется один раз при создании фикстуры, чтобы результаты тестов
    /// не зависели от времени выполнения тестов.
    /// </summary>
    public DateTime Now { get; }

    /// <summary>
    /// Дата, относительно которой построены тестовые данные.
    /// </summary>
    public DateOnly Today { get; }

    /// <summary>
    /// Набор данных фитнес-клуба, общий для фикстуры и сервиса.
    /// </summary>
    public ClubData Data { get; }

    /// <summary>
    /// Справочник специализаций тренеров.
    /// </summary>
    public List<Specialization> Specializations => Data.Specializations;

    /// <summary>
    /// Клиенты фитнес-клуба.
    /// </summary>
    public List<Client> Clients => Data.Clients;

    /// <summary>
    /// Тренеры фитнес-клуба.
    /// </summary>
    public List<Trainer> Trainers => Data.Trainers;

    /// <summary>
    /// Записи клиентов на персональные занятия.
    /// </summary>
    public List<TrainingSession> Sessions => Data.Sessions;

    /// <summary>
    /// Инициализирует тестовые данные: не менее 10 экземпляров каждого класса.
    /// Данные вычисляются относительно зафиксированного момента времени,
    /// поэтому они остаются пригодными независимо от времени запуска тестов.
    /// </summary>
    public FitnessGymFixture()
    {
        Now = DateTime.Now;
        Today = DateOnly.FromDateTime(Now);
        Data = ClubDataSeeder.Create(Now);
    }
}
