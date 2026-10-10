using FitnessGym.Contracts.Dto;

namespace FitnessGym.Contracts.Interfaces;

/// <summary>
/// Сервис фитнес-клуба: операции над сущностями и аналитические запросы.
/// </summary>
public interface IClubService
{
    /// <summary>
    /// Возвращает список всех клиентов.
    /// </summary>
    public IReadOnlyList<ClientDto> GetClients();

    /// <summary>
    /// Возвращает клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    public ClientDto? GetClient(int id);

    /// <summary>
    /// Добавляет клиента и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="client">Данные клиента.</param>
    public ClientDto AddClient(ClientDto client);

    /// <summary>
    /// Заменяет клиента с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    /// <param name="client">Новые данные клиента.</param>
    /// <returns>true, если клиент найден и обновлён.</returns>
    public bool UpdateClient(int id, ClientDto client);

    /// <summary>
    /// Удаляет клиента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор клиента.</param>
    /// <returns>true, если клиент найден и удалён.</returns>
    /// <exception cref="InvalidOperationException">У клиента есть записи на занятия.</exception>
    public bool DeleteClient(int id);

    /// <summary>
    /// Возвращает клиентов с просроченным абонементом, упорядоченных по ФИО.
    /// </summary>
    public IReadOnlyList<ClientDto> GetClientsWithExpiredSubscription();

    /// <summary>
    /// Возвращает список всех тренеров.
    /// </summary>
    public IReadOnlyList<TrainerDto> GetTrainers();

    /// <summary>
    /// Возвращает тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    public TrainerDto? GetTrainer(int id);

    /// <summary>
    /// Добавляет тренера и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="trainer">Данные тренера.</param>
    /// <exception cref="InvalidOperationException">Специализация тренера не найдена.</exception>
    public TrainerDto AddTrainer(TrainerDto trainer);

    /// <summary>
    /// Заменяет тренера с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    /// <param name="trainer">Новые данные тренера.</param>
    /// <returns>true, если тренер найден и обновлён.</returns>
    /// <exception cref="InvalidOperationException">Специализация тренера не найдена.</exception>
    public bool UpdateTrainer(int id, TrainerDto trainer);

    /// <summary>
    /// Удаляет тренера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор тренера.</param>
    /// <returns>true, если тренер найден и удалён.</returns>
    /// <exception cref="InvalidOperationException">У тренера есть записи на занятия.</exception>
    public bool DeleteTrainer(int id);

    /// <summary>
    /// Возвращает тренеров, стаж работы которых не менее указанного количества лет.
    /// </summary>
    /// <param name="minYears">Минимальный стаж работы в годах.</param>
    public IReadOnlyList<TrainerDto> GetExperiencedTrainers(int minYears);

    /// <summary>
    /// Возвращает указанное количество наиболее популярных тренеров
    /// по числу записанных занятий, упорядоченных по убыванию популярности.
    /// </summary>
    /// <param name="count">Количество тренеров в результате.</param>
    public IReadOnlyList<TrainerDto> GetPopularTrainers(int count);

    /// <summary>
    /// Возвращает список всех специализаций (справочник).
    /// </summary>
    public IReadOnlyList<SpecializationDto> GetSpecializations();

    /// <summary>
    /// Возвращает специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    public SpecializationDto? GetSpecialization(int id);

    /// <summary>
    /// Добавляет специализацию и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="specialization">Данные специализации.</param>
    public SpecializationDto AddSpecialization(SpecializationDto specialization);

    /// <summary>
    /// Заменяет специализацию с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    /// <param name="specialization">Новые данные специализации.</param>
    /// <returns>true, если специализация найдена и обновлена.</returns>
    public bool UpdateSpecialization(int id, SpecializationDto specialization);

    /// <summary>
    /// Удаляет специализацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор специализации.</param>
    /// <returns>true, если специализация найдена и удалена.</returns>
    /// <exception cref="InvalidOperationException">Специализацию назначена хотя бы одному тренеру.</exception>
    public bool DeleteSpecialization(int id);

    /// <summary>
    /// Возвращает список всех записей на занятия.
    /// </summary>
    public IReadOnlyList<TrainingSessionDto> GetSessions();

    /// <summary>
    /// Возвращает запись на занятие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    public TrainingSessionDto? GetSession(int id);

    /// <summary>
    /// Добавляет запись на занятие и возвращает созданную сущность с присвоенным идентификатором.
    /// </summary>
    /// <param name="session">Данные записи.</param>
    /// <exception cref="InvalidOperationException">Клиент или тренер записи не найдены.</exception>
    public TrainingSessionDto AddSession(TrainingSessionDto session);

    /// <summary>
    /// Заменяет запись на занятие с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <param name="session">Новые данные записи.</param>
    /// <returns>true, если запись найдена и обновлена.</returns>
    /// <exception cref="InvalidOperationException">Клиент или тренер записи не найдены.</exception>
    public bool UpdateSession(int id, TrainingSessionDto session);

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
    public IReadOnlyList<TrainingSessionDto> GetSessionsForCurrentMonth(string hallName);

    /// <summary>
    /// Определяет, доступен ли зал для записи в указанный момент времени.
    /// Занятия с ещё не проставленным временем окончания не учитываются.
    /// </summary>
    /// <param name="hallName">Название зала.</param>
    /// <param name="at">Момент времени проверки; по умолчанию — текущий момент.</param>
    public bool IsHallAvailable(string hallName, DateTime? at = null);
}
