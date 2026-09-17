using FitnessGym.Domain.Entities;

namespace FitnessGym.Domain.Data;

/// <summary>
/// Набор данных фитнес-клуба для наполнения хранилища в памяти.
/// </summary>
public class ClubData
{
    /// <summary>
    /// Справочник специализаций тренеров.
    /// </summary>
    public required List<Specialization> Specializations { get; init; }

    /// <summary>
    /// Клиенты фитнес-клуба.
    /// </summary>
    public required List<Client> Clients { get; init; }

    /// <summary>
    /// Тренеры фитнес-клуба.
    /// </summary>
    public required List<Trainer> Trainers { get; init; }

    /// <summary>
    /// Записи клиентов на персональные занятия.
    /// </summary>
    public required List<TrainingSession> Sessions { get; init; }
}
