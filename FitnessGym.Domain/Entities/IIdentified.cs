namespace FitnessGym.Domain.Entities;

/// <summary>
/// Сущность с целочисленным идентификатором.
/// </summary>
public interface IIdentified
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; }
}
