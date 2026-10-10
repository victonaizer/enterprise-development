using System.ComponentModel.DataAnnotations;

namespace FitnessGym.Contracts.Dto;

/// <summary>
/// DTO специализации тренера (элемент справочника).
/// </summary>
public class SpecializationDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название специализации.
    /// </summary>
    [Required(ErrorMessage = "Не указано название специализации")]
    [MinLength(2, ErrorMessage = "Название специализации слишком короткое")]
    public string Name { get; set; } = null!;
}
