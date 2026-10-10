using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessGym.Infrastructure.PostgreSql;

/// <summary>
/// Реализация репозитория клиентов поверх Entity Framework Core и PostgreSQL.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
public class PostgreSqlClientRepository(FitnessGymDbContext context) : IClientRepository
{
    /// <inheritdoc/>
    public IReadOnlyList<Client> GetAll() => context.Clients.ToList();

    /// <inheritdoc/>
    public Client? GetById(int id) => context.Clients.FirstOrDefault(c => c.Id == id);

    /// <inheritdoc/>
    public Client Add(Client entity)
    {
        entity.Id = context.Clients.Any() ? context.Clients.Max(c => c.Id) + 1 : 0;
        context.Clients.Add(entity);
        context.SaveChanges();
        return entity;
    }

    /// <inheritdoc/>
    public bool Update(int id, Client entity)
    {
        var existing = context.Clients.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Entry(existing).CurrentValues.SetValues(entity);
        context.SaveChanges();
        return true;
    }

    /// <inheritdoc/>
    public bool Delete(int id)
    {
        var existing = context.Clients.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Clients.Remove(existing);
        context.SaveChanges();
        return true;
    }
}

/// <summary>
/// Реализация репозитория тренеров поверх Entity Framework Core и PostgreSQL.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
public class PostgreSqlTrainerRepository(FitnessGymDbContext context) : ITrainerRepository
{
    /// <inheritdoc/>
    public IReadOnlyList<Trainer> GetAll() =>
        context.Trainers.Include(t => t.Specialization).ToList();

    /// <inheritdoc/>
    public Trainer? GetById(int id) =>
        context.Trainers.Include(t => t.Specialization).FirstOrDefault(t => t.Id == id);

    /// <inheritdoc/>
    public Trainer Add(Trainer entity)
    {
        entity.Id = context.Trainers.Any() ? context.Trainers.Max(t => t.Id) + 1 : 0;
        context.Trainers.Add(entity);
        context.SaveChanges();
        return entity;
    }

    /// <inheritdoc/>
    public bool Update(int id, Trainer entity)
    {
        var existing = context.Trainers.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Entry(existing).CurrentValues.SetValues(entity);
        existing.Specialization = entity.Specialization;
        context.SaveChanges();
        return true;
    }

    /// <inheritdoc/>
    public bool Delete(int id)
    {
        var existing = context.Trainers.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Trainers.Remove(existing);
        context.SaveChanges();
        return true;
    }
}

/// <summary>
/// Реализация репозитория специализаций поверх Entity Framework Core и PostgreSQL.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
public class PostgreSqlSpecializationRepository(FitnessGymDbContext context) : ISpecializationRepository
{
    /// <inheritdoc/>
    public IReadOnlyList<Specialization> GetAll() => context.Specializations.ToList();

    /// <inheritdoc/>
    public Specialization? GetById(int id) => context.Specializations.FirstOrDefault(s => s.Id == id);

    /// <inheritdoc/>
    public Specialization Add(Specialization entity)
    {
        entity.Id = context.Specializations.Any() ? context.Specializations.Max(s => s.Id) + 1 : 0;
        context.Specializations.Add(entity);
        context.SaveChanges();
        return entity;
    }

    /// <inheritdoc/>
    public bool Update(int id, Specialization entity)
    {
        var existing = context.Specializations.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Entry(existing).CurrentValues.SetValues(entity);
        context.SaveChanges();
        return true;
    }

    /// <inheritdoc/>
    public bool Delete(int id)
    {
        var existing = context.Specializations.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Specializations.Remove(existing);
        context.SaveChanges();
        return true;
    }
}

/// <summary>
/// Реализация репозитория записей на занятия поверх Entity Framework Core и PostgreSQL.
/// Навигационные свойства загружаются явно: отложенная загрузка не используется.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
public class PostgreSqlTrainingSessionRepository(FitnessGymDbContext context) : ITrainingSessionRepository
{
    /// <inheritdoc/>
    public IReadOnlyList<TrainingSession> GetAll() =>
        context.Sessions
            .Include(s => s.Client)
            .Include(s => s.Trainer)
            .ThenInclude(t => t.Specialization)
            .ToList();

    /// <inheritdoc/>
    public TrainingSession? GetById(int id) =>
        context.Sessions
            .Include(s => s.Client)
            .Include(s => s.Trainer)
            .ThenInclude(t => t.Specialization)
            .FirstOrDefault(s => s.Id == id);

    /// <inheritdoc/>
    public TrainingSession Add(TrainingSession entity)
    {
        entity.Id = context.Sessions.Any() ? context.Sessions.Max(s => s.Id) + 1 : 0;
        context.Sessions.Add(entity);
        context.SaveChanges();
        return entity;
    }

    /// <inheritdoc/>
    public bool Update(int id, TrainingSession entity)
    {
        var existing = context.Sessions.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Entry(existing).CurrentValues.SetValues(entity);
        existing.Client = entity.Client;
        existing.Trainer = entity.Trainer;
        context.SaveChanges();
        return true;
    }

    /// <inheritdoc/>
    public bool Delete(int id)
    {
        var existing = context.Sessions.Find(id);
        if (existing == null)
        {
            return false;
        }

        context.Sessions.Remove(existing);
        context.SaveChanges();
        return true;
    }
}
