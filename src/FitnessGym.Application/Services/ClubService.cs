using FitnessGym.Contracts.Dto;
using FitnessGym.Contracts.Interfaces;
using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Interfaces;

namespace FitnessGym.Application.Services;

/// <summary>
/// Сервис фитнес-клуба: выполняет CRUD-операции через репозитории,
/// маппинг сущностей в DTO и аналитические запросы.
/// </summary>
/// <param name="clientRepository">Репозиторий клиентов.</param>
/// <param name="trainerRepository">Репозиторий тренеров.</param>
/// <param name="specializationRepository">Репозиторий специализаций.</param>
/// <param name="sessionRepository">Репозиторий записей на занятия.</param>
/// <param name="clock">Функция получения текущего времени; по умолчанию — системное время.
/// Переопределяется в тестах для детерминированности результатов.</param>
public class ClubService(
    IClientRepository clientRepository,
    ITrainerRepository trainerRepository,
    ISpecializationRepository specializationRepository,
    ITrainingSessionRepository sessionRepository,
    Func<DateTime>? clock = null) : IClubService
{
    private readonly Func<DateTime> _clock = clock ?? (() => DateTime.Now);

    /// <inheritdoc/>
    public IReadOnlyList<ClientDto> GetClients() =>
        clientRepository.GetAll().Select(MapToDto).ToList();

    /// <inheritdoc/>
    public ClientDto? GetClient(int id)
    {
        var client = clientRepository.GetById(id);
        return client == null ? null : MapToDto(client);
    }

    /// <inheritdoc/>
    public ClientDto AddClient(ClientDto client) =>
        MapToDto(clientRepository.Add(MapToEntity(client)));

    /// <inheritdoc/>
    public bool UpdateClient(int id, ClientDto client) =>
        clientRepository.Update(id, MapToEntity(client));

    /// <inheritdoc/>
    public bool DeleteClient(int id)
    {
        EnsureClientHasNoSessions(id);
        return clientRepository.Delete(id);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ClientDto> GetClientsWithExpiredSubscription()
    {
        var today = DateOnly.FromDateTime(_clock());
        return clientRepository.GetAll()
            .Where(c => c.SubscriptionEnd < today)
            .OrderBy(c => c.FullName)
            .Select(MapToDto)
            .ToList();
    }

    /// <inheritdoc/>
    public IReadOnlyList<TrainerDto> GetTrainers() =>
        trainerRepository.GetAll().Select(MapToDto).ToList();

    /// <inheritdoc/>
    public TrainerDto? GetTrainer(int id)
    {
        var trainer = trainerRepository.GetById(id);
        return trainer == null ? null : MapToDto(trainer);
    }

    /// <inheritdoc/>
    public TrainerDto AddTrainer(TrainerDto trainer) =>
        MapToDto(trainerRepository.Add(MapToEntity(trainer)));

    /// <inheritdoc/>
    public bool UpdateTrainer(int id, TrainerDto trainer) =>
        trainerRepository.Update(id, MapToEntity(trainer));

    /// <inheritdoc/>
    public bool DeleteTrainer(int id)
    {
        EnsureTrainerHasNoSessions(id);
        return trainerRepository.Delete(id);
    }

    /// <inheritdoc/>
    public IReadOnlyList<TrainerDto> GetExperiencedTrainers(int minYears) =>
        trainerRepository.GetAll()
            .Where(t => t.ExperienceYears >= minYears)
            .Select(MapToDto)
            .ToList();

    /// <inheritdoc/>
    public IReadOnlyList<TrainerDto> GetPopularTrainers(int count) =>
        sessionRepository.GetAll()
            .GroupBy(s => s.Trainer)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => MapToDto(g.Key))
            .ToList();

    /// <inheritdoc/>
    public IReadOnlyList<SpecializationDto> GetSpecializations() =>
        specializationRepository.GetAll().Select(MapToDto).ToList();

    /// <inheritdoc/>
    public SpecializationDto? GetSpecialization(int id)
    {
        var specialization = specializationRepository.GetById(id);
        return specialization == null ? null : MapToDto(specialization);
    }

    /// <inheritdoc/>
    public SpecializationDto AddSpecialization(SpecializationDto specialization) =>
        MapToDto(specializationRepository.Add(MapToEntity(specialization)));

    /// <inheritdoc/>
    public bool UpdateSpecialization(int id, SpecializationDto specialization) =>
        specializationRepository.Update(id, MapToEntity(specialization));

    /// <inheritdoc/>
    public bool DeleteSpecialization(int id)
    {
        EnsureSpecializationHasNoTrainers(id);
        return specializationRepository.Delete(id);
    }

    /// <inheritdoc/>
    public IReadOnlyList<TrainingSessionDto> GetSessions() =>
        sessionRepository.GetAll().Select(MapToDto).ToList();

    /// <inheritdoc/>
    public TrainingSessionDto? GetSession(int id)
    {
        var session = sessionRepository.GetById(id);
        return session == null ? null : MapToDto(session);
    }

    /// <inheritdoc/>
    public TrainingSessionDto AddSession(TrainingSessionDto session) =>
        MapToDto(sessionRepository.Add(MapToEntity(session)));

    /// <inheritdoc/>
    public bool UpdateSession(int id, TrainingSessionDto session) =>
        sessionRepository.Update(id, MapToEntity(session));

    /// <inheritdoc/>
    public bool DeleteSession(int id) => sessionRepository.Delete(id);

    /// <inheritdoc/>
    public IReadOnlyList<TrainingSessionDto> GetSessionsForCurrentMonth(string hallName)
    {
        var now = _clock();
        return sessionRepository.GetAll()
            .Where(s => s.HallName == hallName
                        && s.StartsAt.Year == now.Year
                        && s.StartsAt.Month == now.Month)
            .Select(MapToDto)
            .ToList();
    }

    /// <inheritdoc/>
    public bool IsHallAvailable(string hallName, DateTime? at = null)
    {
        var moment = at ?? _clock();
        return !sessionRepository.GetAll().Any(s =>
            s.HallName == hallName && s.EndsAt != null && s.StartsAt <= moment && moment < s.EndsAt.Value);
    }

    private void EnsureClientHasNoSessions(int clientId)
    {
        if (sessionRepository.GetAll().Any(s => s.Client.Id == clientId))
        {
            throw new InvalidOperationException("У клиента есть записи на занятия — сначала удалите их.");
        }
    }

    private void EnsureTrainerHasNoSessions(int trainerId)
    {
        if (sessionRepository.GetAll().Any(s => s.Trainer.Id == trainerId))
        {
            throw new InvalidOperationException("У тренера есть записи на занятия — сначала удалите их.");
        }
    }

    private void EnsureSpecializationHasNoTrainers(int specializationId)
    {
        if (trainerRepository.GetAll().Any(t => t.Specialization.Id == specializationId))
        {
            throw new InvalidOperationException("Специализация назначена хотя бы одному тренеру — сначала удалите или измените их специализацию.");
        }
    }

    private Client GetClientOrThrow(int id) =>
        clientRepository.GetById(id)
        ?? throw new InvalidOperationException($"Клиент с идентификатором {id} не найден.");

    private Trainer GetTrainerOrThrow(int id) =>
        trainerRepository.GetById(id)
        ?? throw new InvalidOperationException($"Тренер с идентификатором {id} не найден.");

    private Specialization GetSpecializationOrThrow(int id) =>
        specializationRepository.GetById(id)
        ?? throw new InvalidOperationException($"Специализация с идентификатором {id} не найдена.");

    private static ClientDto MapToDto(Client client) => new()
    {
        Id = client.Id,
        PassportNumber = client.PassportNumber,
        FullName = client.FullName,
        Gender = client.Gender,
        BirthDate = client.BirthDate,
        Phone = client.Phone,
        SubscriptionStart = client.SubscriptionStart,
        SubscriptionEnd = client.SubscriptionEnd,
    };

    private static Client MapToEntity(ClientDto dto) => new()
    {
        Id = dto.Id,
        PassportNumber = dto.PassportNumber,
        FullName = dto.FullName,
        Gender = dto.Gender,
        BirthDate = dto.BirthDate,
        Phone = dto.Phone,
        SubscriptionStart = dto.SubscriptionStart,
        SubscriptionEnd = dto.SubscriptionEnd,
    };

    private TrainerDto MapToDto(Trainer trainer) => new()
    {
        Id = trainer.Id,
        PassportNumber = trainer.PassportNumber,
        FullName = trainer.FullName,
        Gender = trainer.Gender,
        BirthDate = trainer.BirthDate,
        SpecializationId = trainer.Specialization.Id,
        SpecializationName = specializationRepository.GetById(trainer.Specialization.Id)?.Name,
        ExperienceYears = trainer.ExperienceYears,
    };

    private Trainer MapToEntity(TrainerDto dto) => new()
    {
        Id = dto.Id,
        PassportNumber = dto.PassportNumber,
        FullName = dto.FullName,
        Gender = dto.Gender,
        BirthDate = dto.BirthDate,
        Specialization = GetSpecializationOrThrow(dto.SpecializationId),
        ExperienceYears = dto.ExperienceYears,
    };

    private static SpecializationDto MapToDto(Specialization specialization) => new()
    {
        Id = specialization.Id,
        Name = specialization.Name,
    };

    private static Specialization MapToEntity(SpecializationDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
    };

    private static TrainingSessionDto MapToDto(TrainingSession session) => new()
    {
        Id = session.Id,
        ClientId = session.Client.Id,
        TrainerId = session.Trainer.Id,
        StartsAt = session.StartsAt,
        EndsAt = session.EndsAt,
        HallName = session.HallName,
        IsTrial = session.IsTrial,
    };

    private TrainingSession MapToEntity(TrainingSessionDto dto) => new()
    {
        Id = dto.Id,
        Client = GetClientOrThrow(dto.ClientId),
        Trainer = GetTrainerOrThrow(dto.TrainerId),
        StartsAt = dto.StartsAt,
        EndsAt = dto.EndsAt,
        HallName = dto.HallName,
        IsTrial = dto.IsTrial,
    };
}
