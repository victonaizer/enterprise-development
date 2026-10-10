using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Interfaces;

namespace FitnessGym.Infrastructure.InMemory;

/// <summary>
/// Базовая реализация IRepository, хранящая сущности в памяти в виде коллекции.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public class InMemoryRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IIdentified
{
    private readonly List<TEntity> _items;
    private int _nextId;

    /// <summary>
    /// Создаёт репозиторий, копируя начальный набор сущностей.
    /// </summary>
    /// <param name="seed">Начальный набор сущностей.</param>
    protected InMemoryRepository(IEnumerable<TEntity> seed)
    {
        _items = [.. seed];
        _nextId = _items.Count == 0 ? 0 : _items.Max(i => i.Id) + 1;
    }

    /// <inheritdoc/>
    public IReadOnlyList<TEntity> GetAll() => _items;

    /// <inheritdoc/>
    public TEntity? GetById(int id) => _items.FirstOrDefault(i => i.Id == id);

    /// <inheritdoc/>
    public TEntity Add(TEntity entity)
    {
        entity.Id = _nextId++;
        _items.Add(entity);
        return entity;
    }

    /// <inheritdoc/>
    public bool Update(int id, TEntity entity)
    {
        var index = _items.FindIndex(i => i.Id == id);
        if (index < 0)
        {
            return false;
        }

        entity.Id = id;
        _items[index] = entity;
        return true;
    }

    /// <inheritdoc/>
    public bool Delete(int id)
    {
        var countRemoved = _items.RemoveAll(i => i.Id == id);
        return countRemoved > 0;
    }
}

/// <summary>
/// Реализация репозитория клиентов поверх коллекции в памяти.
/// </summary>
/// <param name="seed">Начальный набор клиентов.</param>
public class InMemoryClientRepository(IEnumerable<Client> seed)
    : InMemoryRepository<Client>(seed), IClientRepository;

/// <summary>
/// Реализация репозитория тренеров поверх коллекции в памяти.
/// </summary>
/// <param name="seed">Начальный набор тренеров.</param>
public class InMemoryTrainerRepository(IEnumerable<Trainer> seed)
    : InMemoryRepository<Trainer>(seed), ITrainerRepository;

/// <summary>
/// Реализация репозитория специализаций поверх коллекции в памяти.
/// </summary>
/// <param name="seed">Начальный набор специализаций.</param>
public class InMemorySpecializationRepository(IEnumerable<Specialization> seed)
    : InMemoryRepository<Specialization>(seed), ISpecializationRepository;

/// <summary>
/// Реализация репозитория записей на занятия поверх коллекции в памяти.
/// </summary>
/// <param name="seed">Начальный набор записей.</param>
public class InMemoryTrainingSessionRepository(IEnumerable<TrainingSession> seed)
    : InMemoryRepository<TrainingSession>(seed), ITrainingSessionRepository;
