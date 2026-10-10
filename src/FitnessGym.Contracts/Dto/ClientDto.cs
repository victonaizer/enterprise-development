using System.ComponentModel.DataAnnotations;
using FitnessGym.Domain.Enums;

namespace FitnessGym.Contracts.Dto;

/// <summary>
/// DTO клиента фитнес-клуба: используется при создании, обновлении и возврате данных клиента.
/// </summary>
public class ClientDto
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
    /// Номер телефона.
    /// </summary>
    [Required(ErrorMessage = "Не указан телефон")]
    [Phone(ErrorMessage = "Некорректный номер телефона")]
    public string Phone { get; set; } = null!;

    /// <summary>
    /// Дата начала абонемента.
    /// </summary>
    [Required(ErrorMessage = "Не указана дата начала абонемента")]
    public DateOnly SubscriptionStart { get; set; }

    /// <summary>
    /// Дата окончания абонемента.
    /// </summary>
    [Required(ErrorMessage = "Не указана дата окончания абонемента")]
    public DateOnly SubscriptionEnd { get; set; }
}
