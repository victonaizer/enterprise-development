using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessGym.Api.Controllers;

/// <summary>
/// Контроллер CRUD-операций с клиентами фитнес-клуба.
/// </summary>
/// <param name="clubService">Сервис фитнес-клуба.</param>
[ApiController]
[Route("api/clients")]
public class ClientsController(IClubService clubService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех клиентов.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClientDto>), StatusCodes.Status200OK)]
    public IReadOnlyList<ClientDto> GetClients() => clubService.GetClients();

    /// <summary>
    /// Возвращает клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ClientDto> GetClient(int id)
    {
        var client = clubService.GetClient(id);
        return client == null ? NotFound() : client;
    }

    /// <summary>
    /// Создаёт нового клиента и возвращает созданную сущность.
    /// </summary>
    /// <param name="client">Данные клиента.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    public ActionResult<ClientDto> AddClient(ClientDto client)
    {
        var created = clubService.AddClient(client);
        return CreatedAtAction(nameof(GetClient), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновляет клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    /// <param name="client">Новые данные клиента.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateClient(int id, ClientDto client) =>
        clubService.UpdateClient(id, client) ? NoContent() : NotFound();

    /// <summary>
    /// Удаляет клиента по идентификатору.
    /// Удаление невозможно, если у клиента есть записи на занятия.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult DeleteClient(int id)
    {
        try
        {
            return clubService.DeleteClient(id) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Возвращает клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    [HttpGet("expired")]
    [ProducesResponseType(typeof(IReadOnlyList<ClientDto>), StatusCodes.Status200OK)]
    public IReadOnlyList<ClientDto> GetClientsWithExpiredSubscription() =>
        clubService.GetClientsWithExpiredSubscription();
}
