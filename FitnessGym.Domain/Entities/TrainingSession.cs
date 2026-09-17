namespace FitnessGym.Domain.Entities;

/// <summary>
/// Запись клиента на персональное занятие к тренеру.
/// </summary>
public class TrainingSession : IIdentified
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Клиент, записанный на занятие.
    /// </summary>
    public required Client Client { get; set; }

    /// <summary>
    /// Тренер, проводящий занятие.
    /// </summary>
    public required Trainer Trainer { get; set; }

    /// <summary>
    /// Дата и время начала занятия.
    /// </summary>
    public required DateTime StartsAt { get; set; }

    /// <summary>
    /// Дата и время окончания занятия.
    /// В момент записи клиента точное время окончания может быть неизвестно,
    /// поэтому значение может не быть проставлено.
    /// </summary>
    public DateTime? EndsAt { get; set; }

    /// <summary>
    /// Название зала, в котором проходит занятие.
    /// </summary>
    public required string HallName { get; set; }

    /// <summary>
    /// Является ли посещение пробным.
    /// </summary>
    public required bool IsTrial { get; set; }
}
