using FitnessGym.Domain.Data;
using FitnessGym.Domain.Entities;

namespace FitnessGym.Api.Services;

/// <summary>
/// Реализация IClubService, хранящая данные в памяти в виде коллекций.
/// </summary>
/// <param name="data">Набор данных фитнес-клуба для начального наполнения.</param>
/// <param name="clock">Функция получения текущего времени; по умолчанию — системное время.
/// Переопределяется в тестах для детерминированности результатов.</param>
public class ClubService(ClubData data, Func<DateTime>? clock = null) : IClubService
{
    private readonly List<Specialization> _specializations = [.. data.Specializations];
    private readonly List<Client> _clients = [.. data.Clients];
    private readonly List<Trainer> _trainers = [.. data.Trainers];
    private readonly List<TrainingSession> _sessions = [.. data.Sessions];
    private readonly Func<DateTime> _clock = clock ?? (() => DateTime.Now);

    private int _nextSpecializationId = MaxIdOrDefault(data.Specializations, s => s.Id) + 1;
    private int _nextClientId = MaxIdOrDefault(data.Clients, c => c.Id) + 1;
    private int _nextTrainerId = MaxIdOrDefault(data.Trainers, t => t.Id) + 1;
    private int _nextSessionId = MaxIdOrDefault(data.Sessions, s => s.Id) + 1;

    /// <inheritdoc/>
    public IReadOnlyList<Client> GetClients() => _clients;

    /// <inheritdoc/>
    public Client? GetClient(int id) => _clients.FirstOrDefault(c => c.Id == id);

    /// <inheritdoc/>
    public Client AddClient(Client client)
    {
        client.Id = _nextClientId++;
        _clients.Add(client);
        return client;
    }

    /// <inheritdoc/>
    public bool UpdateClient(int id, Client client)
    {
        var index = _clients.FindIndex(c => c.Id == id);
        if (index < 0)
        {
            return false;
        }

        client.Id = id;
        _clients[index] = client;
        return true;
    }

    /// <inheritdoc/>
    public bool DeleteClient(int id) => RemoveById(_clients, id);

    /// <inheritdoc/>
    public IReadOnlyList<Client> GetClientsWithExpiredSubscription()
    {
        var today = DateOnly.FromDateTime(_clock());
        return _clients
            .Where(c => c.SubscriptionEnd < today)
            .OrderBy(c => c.FullName)
            .ToList();
    }

    /// <inheritdoc/>
    public IReadOnlyList<Trainer> GetTrainers() => _trainers;

    /// <inheritdoc/>
    public Trainer? GetTrainer(int id) => _trainers.FirstOrDefault(t => t.Id == id);

    /// <inheritdoc/>
    public Trainer AddTrainer(Trainer trainer)
    {
        trainer.Id = _nextTrainerId++;
        _trainers.Add(trainer);
        return trainer;
    }

    /// <inheritdoc/>
    public bool UpdateTrainer(int id, Trainer trainer)
    {
        var index = _trainers.FindIndex(t => t.Id == id);
        if (index < 0)
        {
            return false;
        }

        trainer.Id = id;
        _trainers[index] = trainer;
        return true;
    }

    /// <inheritdoc/>
    public bool DeleteTrainer(int id) => RemoveById(_trainers, id);

    /// <inheritdoc/>
    public IReadOnlyList<Trainer> GetExperiencedTrainers(int minYears) =>
        _trainers.Where(t => t.ExperienceYears >= minYears).ToList();

    /// <inheritdoc/>
    public IReadOnlyList<Trainer> GetPopularTrainers(int count) =>
        _sessions
            .GroupBy(s => s.Trainer)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToList();

    /// <inheritdoc/>
    public IReadOnlyList<Specialization> GetSpecializations() => _specializations;

    /// <inheritdoc/>
    public Specialization? GetSpecialization(int id) =>
        _specializations.FirstOrDefault(s => s.Id == id);

    /// <inheritdoc/>
    public Specialization AddSpecialization(Specialization specialization)
    {
        specialization.Id = _nextSpecializationId++;
        _specializations.Add(specialization);
        return specialization;
    }

    /// <inheritdoc/>
    public bool UpdateSpecialization(int id, Specialization specialization)
    {
        var index = _specializations.FindIndex(s => s.Id == id);
        if (index < 0)
        {
            return false;
        }

        specialization.Id = id;
        _specializations[index] = specialization;
        return true;
    }

    /// <inheritdoc/>
    public bool DeleteSpecialization(int id) => RemoveById(_specializations, id);

    /// <inheritdoc/>
    public IReadOnlyList<TrainingSession> GetSessions() => _sessions;

    /// <inheritdoc/>
    public TrainingSession? GetSession(int id) => _sessions.FirstOrDefault(s => s.Id == id);

    /// <inheritdoc/>
    public TrainingSession AddSession(TrainingSession session)
    {
        session.Id = _nextSessionId++;
        _sessions.Add(session);
        return session;
    }

    /// <inheritdoc/>
    public bool UpdateSession(int id, TrainingSession session)
    {
        var index = _sessions.FindIndex(s => s.Id == id);
        if (index < 0)
        {
            return false;
        }

        session.Id = id;
        _sessions[index] = session;
        return true;
    }

    /// <inheritdoc/>
    public bool DeleteSession(int id) => RemoveById(_sessions, id);

    /// <inheritdoc/>
    public IReadOnlyList<TrainingSession> GetSessionsForCurrentMonth(string hallName)
    {
        var now = _clock();
        return _sessions
            .Where(s => s.HallName == hallName
                        && s.StartsAt.Year == now.Year
                        && s.StartsAt.Month == now.Month)
            .ToList();
    }

    /// <inheritdoc/>
    public bool IsHallAvailable(string hallName, DateTime? at = null)
    {
        var moment = at ?? _clock();
        return !_sessions.Any(s =>
            s.HallName == hallName && s.EndsAt != null && s.StartsAt <= moment && moment < s.EndsAt.Value);
    }

    private static bool RemoveById<T>(List<T> items, int id) where T : IIdentified
    {
        var countRemoved = items.RemoveAll(item => item.Id == id);
        return countRemoved > 0;
    }

    private static int MaxIdOrDefault<T>(IEnumerable<T> items, Func<T, int> idSelector) =>
        items.Any() ? items.Max(idSelector) : 0;
}
