using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessGym.Api.Controllers;

/// <summary>
/// Контроллер CRUD-операций со справочником специализаций тренеров.
/// </summary>
/// <param name="clubService">Сервис фитнес-клуба.</param>
[ApiController]
[Route("api/specializations")]
public class SpecializationsController(IClubService clubService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех специализаций.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SpecializationDto>), StatusCodes.Status200OK)]
    public IReadOnlyList<SpecializationDto> GetSpecializations() => clubService.GetSpecializations();

    /// <summary>
    /// Возвращает специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SpecializationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SpecializationDto> GetSpecialization(int id)
    {
        var specialization = clubService.GetSpecialization(id);
        return specialization == null ? NotFound() : specialization;
    }

    /// <summary>
    /// Создаёт новую специализацию и возвращает созданную сущность.
    /// </summary>
    /// <param name="specialization">Данные специализации.</param>
    [HttpPost]
    [ProducesResponseType(typeof(SpecializationDto), StatusCodes.Status201Created)]
    public ActionResult<SpecializationDto> AddSpecialization(SpecializationDto specialization)
    {
        var created = clubService.AddSpecialization(specialization);
        return CreatedAtAction(nameof(GetSpecialization), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновляет специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    /// <param name="specialization">Новые данные специализации.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateSpecialization(int id, SpecializationDto specialization) =>
        clubService.UpdateSpecialization(id, specialization) ? NoContent() : NotFound();

    /// <summary>
    /// Удаляет специализацию по идентификатору.
    /// Удаление невозможно, если специализация назначена хотя бы одному тренеру.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult DeleteSpecialization(int id)
    {
        try
        {
            return clubService.DeleteSpecialization(id) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
