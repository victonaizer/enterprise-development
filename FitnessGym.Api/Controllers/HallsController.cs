using FitnessGym.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessGym.Api.Controllers;

/// <summary>
/// Контроллер проверки доступности залов фитнес-клуба для записи.
/// </summary>
/// <param name="clubService">Сервис фитнес-клуба.</param>
[ApiController]
[Route("api/halls")]
public class HallsController(IClubService clubService) : ControllerBase
{
    /// <summary>
    /// Возвращает названия всех залов, встречающихся в записях на занятия.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    public IReadOnlyList<string> GetHalls() =>
        clubService.GetSessions().Select(s => s.HallName).Distinct().ToList();

    /// <summary>
    /// Определяет, доступен ли зал для записи в указанный момент времени.
    /// Занятия с ещё не проставленным временем окончания не учитываются.
    /// </summary>
    /// <param name="hallName">Название зала.</param>
    /// <param name="at">Момент времени проверки; по умолчанию — текущий момент.</param>
    [HttpGet("{hallName}/availability")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public bool IsHallAvailable(string hallName, [FromQuery] DateTime? at) =>
        clubService.IsHallAvailable(hallName, at);
}
