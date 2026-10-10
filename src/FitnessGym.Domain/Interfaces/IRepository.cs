using FitnessGym.Domain.Entities;

namespace FitnessGym.Domain.Interfaces;

/// <summary>
/// Базовый репозиторий: абстракция доступа к хранилищу сущностей с целочисленным идентификатором.
/// Позволяет заменить способ хранения данных (в памяти, в базе данных), не изменяя остальной код.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRepository<TEntity> where TEntity : IIdentified
{
    /// <summary>
    /// Возвращает все сущности хранилища.
    /// </summary>
    public IReadOnlyList<TEntity> GetAll();

    /// <summary>
    /// Возвращает сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    public TEntity? GetById(int id);

    /// <summary>
    /// Добавляет сущность, присваивая ей новый идентификатор.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    /// <returns>Добавленная сущность с присвоенным идентификатором.</returns>
    public TEntity Add(TEntity entity);

    /// <summary>
    /// Заменяет сущность с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="entity">Новые данные сущности.</param>
    /// <returns>true, если сущность найдена и обновлена.</returns>
    public bool Update(int id, TEntity entity);

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>true, если сущность найдена и удалена.</returns>
    public bool Delete(int id);
}
