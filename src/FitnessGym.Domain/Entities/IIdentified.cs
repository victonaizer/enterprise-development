namespace FitnessGym.Domain.Entities;

/// <summary>
/// Сущность с целочисленным идентификатором.
/// </summary>
public interface IIdentified
{
    /// <summary>
    /// Идентификатор; присваивается репозиторием при добавлении сущности.
    /// </summary>
    public int Id { get; set; }
}
