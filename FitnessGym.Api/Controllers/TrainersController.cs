using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessGym.Api.Controllers;

/// <summary>
/// Контроллер CRUD-операций с тренерами фитнес-клуба и аналитических запросов по тренерам.
/// </summary>
/// <param name="clubService">Сервис фитнес-клуба.</param>
[ApiController]
[Route("api/trainers")]
public class TrainersController(IClubService clubService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех тренеров.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Trainer>), StatusCodes.Status200OK)]
    public IReadOnlyList<Trainer> GetTrainers() => clubService.GetTrainers();

    /// <summary>
    /// Возвращает тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Trainer> GetTrainer(int id)
    {
        var trainer = clubService.GetTrainer(id);
        return trainer == null ? NotFound() : trainer;
    }

    /// <summary>
    /// Создаёт нового тренера и возвращает созданную сущность.
    /// </summary>
    /// <param name="trainer">Данные тренера.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Trainer), StatusCodes.Status201Created)]
    public ActionResult<Trainer> AddTrainer(Trainer trainer)
    {
        var created = clubService.AddTrainer(trainer);
        return CreatedAtAction(nameof(GetTrainer), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновляет тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    /// <param name="trainer">Новые данные тренера.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateTrainer(int id, Trainer trainer) =>
        clubService.UpdateTrainer(id, trainer) ? NoContent() : NotFound();

    /// <summary>
    /// Удаляет тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteTrainer(int id) =>
        clubService.DeleteTrainer(id) ? NoContent() : NotFound();

    /// <summary>
    /// Возвращает тренеров, стаж работы которых не менее указанного количества лет.
    /// </summary>
    /// <param name="minYears">Минимальный стаж работы в годах; по умолчанию 5.</param>
    [HttpGet("experienced")]
    [ProducesResponseType(typeof(IReadOnlyList<Trainer>), StatusCodes.Status200OK)]
    public IReadOnlyList<Trainer> GetExperiencedTrainers([FromQuery] int minYears = 5) =>
        clubService.GetExperiencedTrainers(minYears);

    /// <summary>
    /// Возвращает наиболее популярных тренеров по числу записанных занятий,
    /// упорядоченных по убыванию популярности.
    /// </summary>
    /// <param name="count">Количество тренеров в ответе; по умолчанию 5.</param>
    [HttpGet("popular")]
    [ProducesResponseType(typeof(IReadOnlyList<Trainer>), StatusCodes.Status200OK)]
    public IReadOnlyList<Trainer> GetPopularTrainers([FromQuery] int count = 5) =>
        clubService.GetPopularTrainers(count);
}
