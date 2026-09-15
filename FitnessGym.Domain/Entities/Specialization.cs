namespace FitnessGym.Domain.Entities;

/// <summary>
/// Специализация тренера (справочник).
/// </summary>
public class Specialization
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
