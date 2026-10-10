using System.ComponentModel.DataAnnotations;
using FitnessGym.Domain.Enums;

namespace FitnessGym.Contracts.Dto;

/// <summary>
/// DTO тренера фитнес-клуба: используется при создании, обновлении и возврате данных тренера.
/// </summary>
public class TrainerDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер паспорта.
    /// </summary>
    [Required(ErrorMessage = "Не указан номер паспорта")]
    public string PassportNumber { get; set; } = null!;

    /// <summary>
    /// ФИО.
    /// </summary>
    [Required(ErrorMessage = "Не указано ФИО")]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Пол.
    /// </summary>
    [Required(ErrorMessage = "Не указан пол")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Некорректное значение пола")]
    public Gender Gender { get; set; }

    /// <summary>
    /// Дата рождения.
    /// </summary>
    [Required(ErrorMessage = "Не указана дата рождения")]
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// Идентификатор специализации тренера (ссылка на справочник).
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Некорректный идентификатор специализации")]
    public int SpecializationId { get; set; }

    /// <summary>
    /// Название специализации; заполняется только в ответах сервера.
    /// </summary>
    public string? SpecializationName { get; set; }

    /// <summary>
    /// Стаж работы в годах.
    /// </summary>
    [Range(0, 100, ErrorMessage = "Стаж работы должен быть от 0 до 100 лет")]
    public int ExperienceYears { get; set; }
}
