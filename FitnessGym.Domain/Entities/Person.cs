using FitnessGym.Domain.Enums;

namespace FitnessGym.Domain.Entities;

/// <summary>
/// Человек — базовый класс с персональными данными, общими для клиента и тренера.
/// </summary>
public abstract class Person : IIdentified
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер паспорта.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// ФИО.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Пол.
    /// </summary>
    public required Gender Gender { get; set; }

    /// <summary>
    /// Дата рождения.
    /// </summary>
    public required DateOnly BirthDate { get; set; }
}
