namespace FitnessGym.Domain.Entities;

/// <summary>
/// Специализация тренера (справочник).
/// </summary>
public class Specialization : IIdentified
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название специализации.
    /// </summary>
    public required string Name { get; set; }
}
