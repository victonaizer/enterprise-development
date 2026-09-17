using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;
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
    [ProducesResponseType(typeof(IReadOnlyList<Specialization>), StatusCodes.Status200OK)]
    public IReadOnlyList<Specialization> GetSpecializations() => clubService.GetSpecializations();

    /// <summary>
    /// Возвращает специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Specialization), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Specialization> GetSpecialization(int id)
    {
        var specialization = clubService.GetSpecialization(id);
        return specialization == null ? NotFound() : specialization;
    }

    /// <summary>
    /// Создаёт новую специализацию и возвращает созданную сущность.
    /// </summary>
    /// <param name="specialization">Данные специализации.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Specialization), StatusCodes.Status201Created)]
    public ActionResult<Specialization> AddSpecialization(Specialization specialization)
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
    public IActionResult UpdateSpecialization(int id, Specialization specialization) =>
        clubService.UpdateSpecialization(id, specialization) ? NoContent() : NotFound();

    /// <summary>
    /// Удаляет специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteSpecialization(int id) =>
        clubService.DeleteSpecialization(id) ? NoContent() : NotFound();
}
