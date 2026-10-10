using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
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
    [ProducesResponseType(typeof(IReadOnlyList<TrainerDto>), StatusCodes.Status200OK)]
    public IReadOnlyList<TrainerDto> GetTrainers() => clubService.GetTrainers();

    /// <summary>
    /// Возвращает тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TrainerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TrainerDto> GetTrainer(int id)
    {
        var trainer = clubService.GetTrainer(id);
        return trainer == null ? NotFound() : trainer;
    }

    /// <summary>
    /// Создаёт нового тренера и возвращает созданную сущность.
    /// </summary>
    /// <param name="trainer">Данные тренера.</param>
    [HttpPost]
    [ProducesResponseType(typeof(TrainerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TrainerDto> AddTrainer(TrainerDto trainer)
    {
        try
        {
            var created = clubService.AddTrainer(trainer);
            return CreatedAtAction(nameof(GetTrainer), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Обновляет тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    /// <param name="trainer">Новые данные тренера.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateTrainer(int id, TrainerDto trainer)
    {
        try
        {
            return clubService.UpdateTrainer(id, trainer) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет тренера по идентификатору.
    /// Удаление невозможно, если у тренера есть записи на занятия.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult DeleteTrainer(int id)
    {
        try
        {
            return clubService.DeleteTrainer(id) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Возвращает тренеров, стаж работы которых не менее указанного количества лет.
    /// </summary>
    /// <param name="minYears">Минимальный стаж работы в годах; по умолчанию 5.</param>
    [HttpGet("experienced")]
    [ProducesResponseType(typeof(IReadOnlyList<TrainerDto>), StatusCodes.Status200OK)]
    public IReadOnlyList<TrainerDto> GetExperiencedTrainers([FromQuery] int minYears = 5) =>
        clubService.GetExperiencedTrainers(minYears);

    /// <summary>
    /// Возвращает наиболее популярных тренеров по числу записанных занятий,
    /// упорядоченных по убыванию популярности.
    /// </summary>
    /// <param name="count">Количество тренеров в ответе; по умолчанию 5.</param>
    [HttpGet("popular")]
    [ProducesResponseType(typeof(IReadOnlyList<TrainerDto>), StatusCodes.Status200OK)]
    public IReadOnlyList<TrainerDto> GetPopularTrainers([FromQuery] int count = 5) =>
        clubService.GetPopularTrainers(count);
}
