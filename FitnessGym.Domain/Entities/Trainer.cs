namespace FitnessGym.Domain.Entities;

/// <summary>
/// Тренер фитнес-клуба.
/// </summary>
public class Trainer : Person
{
    /// <summary>
    /// Специализация (справочная сущность).
    /// </summary>
    public required Specialization Specialization { get; set; }

    /// <summary>
    /// Стаж работы в годах.
    /// </summary>
    public required int ExperienceYears { get; set; }
}
