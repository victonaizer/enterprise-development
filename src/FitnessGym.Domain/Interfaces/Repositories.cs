using FitnessGym.Domain.Entities;

namespace FitnessGym.Domain.Interfaces;

/// <summary>
/// Репозиторий клиентов фитнес-клуба.
/// </summary>
public interface IClientRepository : IRepository<Client>
{
}

/// <summary>
/// Репозиторий тренеров фитнес-клуба.
/// </summary>
public interface ITrainerRepository : IRepository<Trainer>
{
}

/// <summary>
/// Репозиторий справочника специализаций тренеров.
/// </summary>
public interface ISpecializationRepository : IRepository<Specialization>
{
}

/// <summary>
/// Репозиторий записей клиентов на персональные занятия.
/// </summary>
public interface ITrainingSessionRepository : IRepository<TrainingSession>
{
}
