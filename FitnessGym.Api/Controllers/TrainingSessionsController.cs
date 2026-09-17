using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessGym.Api.Controllers;

/// <summary>
/// Контроллер CRUD-операций с записями клиентов на занятия и аналитических запросов по занятиям.
/// </summary>
/// <param name="clubService">Сервис фитнес-клуба.</param>
[ApiController]
[Route("api/sessions")]
public class TrainingSessionsController(IClubService clubService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех записей на занятия.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TrainingSession>), StatusCodes.Status200OK)]
    public IReadOnlyList<TrainingSession> GetSessions() => clubService.GetSessions();

    /// <summary>
    /// Возвращает запись на занятие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TrainingSession), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TrainingSession> GetSession(int id)
    {
        var session = clubService.GetSession(id);
        return session == null ? NotFound() : session;
    }

    /// <summary>
    /// Создаёт новую запись на занятие и возвращает созданную сущность.
    /// </summary>
    /// <param name="session">Данные записи.</param>
    [HttpPost]
    [ProducesResponseType(typeof(TrainingSession), StatusCodes.Status201Created)]
    public ActionResult<TrainingSession> AddSession(TrainingSession session)
    {
        var created = clubService.AddSession(session);
        return CreatedAtAction(nameof(GetSession), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновляет запись на занятие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <param name="session">Новые данные записи.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateSession(int id, TrainingSession session) =>
        clubService.UpdateSession(id, session) ? NoContent() : NotFound();

    /// <summary>
    /// Удаляет запись на занятие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteSession(int id) =>
        clubService.DeleteSession(id) ? NoContent() : NotFound();

    /// <summary>
    /// Возвращает занятия в выбранном зале за текущий месяц.
    /// </summary>
    /// <param name="hallName">Название зала.</param>
    [HttpGet("current-month")]
    [ProducesResponseType(typeof(IReadOnlyList<TrainingSession>), StatusCodes.Status200OK)]
    public IReadOnlyList<TrainingSession> GetSessionsForCurrentMonth([FromQuery] string hallName) =>
        clubService.GetSessionsForCurrentMonth(hallName);
}
