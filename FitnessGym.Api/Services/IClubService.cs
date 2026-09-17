using FitnessGym.Domain.Entities;

namespace FitnessGym.Api.Services;

/// <summary>
/// Сервис фитнес-клуба: управление сущностями и выполнение аналитических запросов.
/// </summary>
public interface IClubService
{
    /// <summary>
    /// Возвращает список всех клиентов.
    /// </summary>
    public IReadOnlyList<Client> GetClients();

    /// <summary>
    /// Возвращает клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    public Client? GetClient(int id);

    /// <summary>
    /// Добавляет клиента и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="client">Клиент для добавления.</param>
    public Client AddClient(Client client);

    /// <summary>
    /// Заменяет клиента с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    /// <param name="client">Новые данные клиента.</param>
    /// <returns>true, если клиент найден и обновлён.</returns>
    public bool UpdateClient(int id, Client client);

    /// <summary>
    /// Удаляет клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    /// <returns>true, если клиент найден и удалён.</returns>
    public bool DeleteClient(int id);

    /// <summary>
    /// Возвращает клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    public IReadOnlyList<Client> GetClientsWithExpiredSubscription();

    /// <summary>
    /// Возвращает список всех тренеров.
    /// </summary>
    public IReadOnlyList<Trainer> GetTrainers();

    /// <summary>
    /// Возвращает тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    public Trainer? GetTrainer(int id);

    /// <summary>
    /// Добавляет тренера и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="trainer">Тренер для добавления.</param>
    public Trainer AddTrainer(Trainer trainer);

    /// <summary>
    /// Заменяет тренера с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    /// <param name="trainer">Новые данные тренера.</param>
    /// <returns>true, если тренер найден и обновлён.</returns>
    public bool UpdateTrainer(int id, Trainer trainer);

    /// <summary>
    /// Удаляет тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    /// <returns>true, если тренер найден и удалён.</returns>
    public bool DeleteTrainer(int id);

    /// <summary>
    /// Возвращает тренеров, стаж работы которых не менее указанного количества лет.
    /// </summary>
    /// <param name="minYears">Минимальный стаж работы в годах.</param>
    public IReadOnlyList<Trainer> GetExperiencedTrainers(int minYears);

    /// <summary>
    /// Возвращает указанное количество наиболее популярных тренеров
    /// по числу записанных занятий, упорядоченных по убыванию популярности.
    /// </summary>
    /// <param name="count">Количество тренеров в результате.</param>
    public IReadOnlyList<Trainer> GetPopularTrainers(int count);

    /// <summary>
    /// Возвращает список всех специализаций (справочник).
    /// </summary>
    public IReadOnlyList<Specialization> GetSpecializations();

    /// <summary>
    /// Возвращает специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    public Specialization? GetSpecialization(int id);

    /// <summary>
    /// Добавляет специализацию и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="specialization">Специализация для добавления.</param>
    public Specialization AddSpecialization(Specialization specialization);

    /// <summary>
    /// Заменяет специализацию с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    /// <param name="specialization">Новые данные специализации.</param>
    /// <returns>true, если специализация найдена и обновлена.</returns>
    public bool UpdateSpecialization(int id, Specialization specialization);

    /// <summary>
    /// Удаляет специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    /// <returns>true, если специализация найдена и удалена.</returns>
    public bool DeleteSpecialization(int id);

    /// <summary>
    /// Возвращает список всех записей на занятия.
    /// </summary>
    public IReadOnlyList<TrainingSession> GetSessions();

    /// <summary>
    /// Возвращает запись на занятие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    public TrainingSession? GetSession(int id);

    /// <summary>
    /// Добавляет запись на занятие и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="session">Запись для добавления.</param>
    public TrainingSession AddSession(TrainingSession session);

    /// <summary>
    /// Заменяет запись на занятие с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <param name="session">Новые данные записи.</param>
    /// <returns>true, если запись найдена и обновлена.</returns>
    public bool UpdateSession(int id, TrainingSession session);

    /// <summary>
    /// Удаляет запись на занятие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <returns>true, если запись найдена и удалена.</returns>
    public bool DeleteSession(int id);

    /// <summary>
    /// Возвращает занятия в выбранном зале за текущий месяц.
    /// </summary>
    /// <param name="hallName">Название зала.</param>
    public IReadOnlyList<TrainingSession> GetSessionsForCurrentMonth(string hallName);

    /// <summary>
    /// Определяет, доступен ли зал для записи в указанный момент времени.
    /// Занятия с ещё не проставленным временем окончания не учитываются.
    /// </summary>
    /// <param name="hallName">Название зала.</param>
    /// <param name="at">Момент времени проверки; по умолчанию — текущий момент.</param>
    public bool IsHallAvailable(string hallName, DateTime? at = null);
}
