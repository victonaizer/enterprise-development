using System.ComponentModel.DataAnnotations;

namespace FitnessGym.Contracts.Dto;

/// <summary>
/// DTO записи клиента на персональное занятие.
/// </summary>
public class TrainingSessionDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Некорректный идентификатор клиента")]
    public int ClientId { get; set; }

    /// <summary>
    /// Идентификатор тренера.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Некорректный идентификатор тренера")]
    public int TrainerId { get; set; }

    /// <summary>
    /// Дата и время начала занятия.
    /// </summary>
    [Required(ErrorMessage = "Не указано время начала занятия")]
    public DateTime StartsAt { get; set; }

    /// <summary>
    /// Дата и время окончания занятия; может быть не проставлена.
    /// </summary>
    public DateTime? EndsAt { get; set; }

    /// <summary>
    /// Название зала, в котором проходит занятие.
    /// </summary>
    [Required(ErrorMessage = "Не указано название зала")]
    public string HallName { get; set; } = null!;

    /// <summary>
    /// Является ли посещение пробным.
    /// </summary>
    public bool IsTrial { get; set; }
}
