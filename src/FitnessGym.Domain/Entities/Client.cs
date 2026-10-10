namespace FitnessGym.Domain.Entities;

/// <summary>
/// Клиент фитнес-клуба.
/// </summary>
public class Client : Person
{
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// Дата начала абонемента.
    /// </summary>
    public required DateOnly SubscriptionStart { get; set; }

    /// <summary>
    /// Дата окончания абонемента.
    /// </summary>
    public required DateOnly SubscriptionEnd { get; set; }
}
