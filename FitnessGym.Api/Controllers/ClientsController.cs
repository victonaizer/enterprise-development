using FitnessGym.Api.Services;
using FitnessGym.Domain.Entities;
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
    [ProducesResponseType(typeof(IReadOnlyList<Client>), StatusCodes.Status200OK)]
    public IReadOnlyList<Client> GetClients() => clubService.GetClients();

    /// <summary>
    /// Возвращает клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Client> GetClient(int id)
    {
        var client = clubService.GetClient(id);
        return client == null ? NotFound() : client;
    }

    /// <summary>
    /// Создаёт нового клиента и возвращает созданную сущность.
    /// </summary>
    /// <param name="client">Данные клиента.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Client), StatusCodes.Status201Created)]
    public ActionResult<Client> AddClient(Client client)
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
    public IActionResult UpdateClient(int id, Client client) =>
        clubService.UpdateClient(id, client) ? NoContent() : NotFound();

    /// <summary>
    /// Удаляет клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteClient(int id) =>
        clubService.DeleteClient(id) ? NoContent() : NotFound();

    /// <summary>
    /// Возвращает клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    [HttpGet("expired")]
    [ProducesResponseType(typeof(IReadOnlyList<Client>), StatusCodes.Status200OK)]
    public IReadOnlyList<Client> GetClientsWithExpiredSubscription() =>
        clubService.GetClientsWithExpiredSubscription();
}
