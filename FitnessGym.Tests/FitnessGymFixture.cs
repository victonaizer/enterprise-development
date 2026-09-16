using FitnessGym.Domain.Entities;
using FitnessGym.Domain.Enums;

namespace FitnessGym.Tests;

/// <summary>
/// Фикстура, содержащая тестовые данные фитнес-клуба.
/// </summary>
public class FitnessGymFixture
{
    /// <summary>
    /// Момент времени, относительно которого построены тестовые данные.
    /// Фиксируется один раз при создании фикстуры, чтобы результаты тестов
    /// не зависели от времени выполнения тестов.
    /// </summary>
    public DateTime Now { get; }

    /// <summary>
    /// Дата, относительно которой построены тестовые данные.
    /// </summary>
    public DateOnly Today { get; }

    /// <summary>
    /// Справочник специализаций тренеров.
    /// </summary>
    public readonly List<Specialization> Specializations;

    /// <summary>
    /// Клиенты фитнес-клуба.
    /// </summary>
    public readonly List<Client> Clients;

    /// <summary>
    /// Тренеры фитнес-клуба.
    /// </summary>
    public readonly List<Trainer> Trainers;

    /// <summary>
    /// Записи клиентов на персональные занятия.
    /// </summary>
    public readonly List<TrainingSession> Sessions;

    /// <summary>
    /// Инициализирует тестовые данные: не менее 10 экземпляров каждого класса.
    /// Даты абонементов и занятий вычисляются относительно зафиксированного момента времени,
    /// поэтому данные остаются пригодными независимо от времени запуска тестов.
    /// </summary>
    public FitnessGymFixture()
    {
        Now = DateTime.Now;
        Today = DateOnly.FromDateTime(Now);
        var currentMonth = new DateTime(Now.Year, Now.Month, 1);
        var previousMonth = currentMonth.AddMonths(-1);
        var nextMonth = currentMonth.AddMonths(1);

        static DateTime At(DateTime month, int day, int hour) =>
            new(month.Year, month.Month, day, hour, 0, 0);

        Specializations =
        [
            new Specialization { Id = 0, Name = "Йога" },
            new Specialization { Id = 1, Name = "Пилатес" },
            new Specialization { Id = 2, Name = "Кроссфит" },
            new Specialization { Id = 3, Name = "Плавание" },
            new Specialization { Id = 4, Name = "Бокс" },
            new Specialization { Id = 5, Name = "Тяжёлая атлетика" },
            new Specialization { Id = 6, Name = "Стретчинг" },
            new Specialization { Id = 7, Name = "Функциональный тренинг" },
            new Specialization { Id = 8, Name = "Лёгкая атлетика" },
            new Specialization { Id = 9, Name = "Реабилитация" },
        ];

        Clients =
        [
            // Клиенты с просроченным абонементом
            new Client { Id = 0, PassportNumber = "4501 123456", FullName = "Андреев Алексей Сергеевич", Gender = Gender.Male, BirthDate = new DateOnly(1990, 4, 12), Phone = "+7 (911) 100-00-01", SubscriptionStart = Today.AddDays(-60), SubscriptionEnd = Today.AddDays(-30) },
            new Client { Id = 1, PassportNumber = "4502 234567", FullName = "Борисова Марина Владимировна", Gender = Gender.Female, BirthDate = new DateOnly(1985, 11, 25), Phone = "+7 (911) 100-00-02", SubscriptionStart = Today.AddDays(-90), SubscriptionEnd = Today.AddDays(-7) },
            new Client { Id = 2, PassportNumber = "4503 345678", FullName = "Васильев Дмитрий Олегович", Gender = Gender.Male, BirthDate = new DateOnly(1978, 2, 18), Phone = "+7 (911) 100-00-03", SubscriptionStart = Today.AddDays(-120), SubscriptionEnd = Today.AddDays(-1) },
            new Client { Id = 3, PassportNumber = "4504 456789", FullName = "Гаврилова Елена Николаевна", Gender = Gender.Female, BirthDate = new DateOnly(1992, 6, 30), Phone = "+7 (911) 100-00-04", SubscriptionStart = Today.AddDays(-445), SubscriptionEnd = Today.AddDays(-45) },
            new Client { Id = 4, PassportNumber = "4505 567890", FullName = "Дмитриев Павел Андреевич", Gender = Gender.Male, BirthDate = new DateOnly(1996, 9, 5), Phone = "+7 (911) 100-00-05", SubscriptionStart = Today.AddDays(-74), SubscriptionEnd = Today.AddDays(-14) },
            // Клиенты с действующим абонементом
            new Client { Id = 5, PassportNumber = "4506 678901", FullName = "Егорова Ольга Ивановна", Gender = Gender.Female, BirthDate = new DateOnly(1983, 12, 1), Phone = "+7 (911) 100-00-06", SubscriptionStart = Today.AddDays(-90), SubscriptionEnd = Today.AddDays(90) },
            new Client { Id = 6, PassportNumber = "4507 789012", FullName = "Жуков Игорь Валерьевич", Gender = Gender.Male, BirthDate = new DateOnly(1994, 5, 22), Phone = "+7 (911) 100-00-07", SubscriptionStart = Today.AddDays(-30), SubscriptionEnd = Today.AddDays(30) },
            new Client { Id = 7, PassportNumber = "4508 890123", FullName = "Зотова Анна Викторовна", Gender = Gender.Female, BirthDate = new DateOnly(1999, 8, 17), Phone = "+7 (911) 100-00-08", SubscriptionStart = Today.AddDays(-60), SubscriptionEnd = Today.AddDays(15) },
            new Client { Id = 8, PassportNumber = "4509 901234", FullName = "Иванов Сергей Петрович", Gender = Gender.Male, BirthDate = new DateOnly(1975, 1, 9), Phone = "+7 (911) 100-00-09", SubscriptionStart = Today.AddDays(-10), SubscriptionEnd = Today.AddDays(80) },
            new Client { Id = 9, PassportNumber = "4510 012345", FullName = "Козлова Мария Дмитриевна", Gender = Gender.Female, BirthDate = new DateOnly(2001, 3, 28), Phone = "+7 (911) 100-00-10", SubscriptionStart = Today.AddDays(-1), SubscriptionEnd = Today.AddDays(364) },
            // Клиенты, абонемент которых ещё не начался
            new Client { Id = 10, PassportNumber = "4511 111111", FullName = "Лебедев Артём Игоревич", Gender = Gender.Male, BirthDate = new DateOnly(2003, 7, 7), Phone = "+7 (911) 100-00-11", SubscriptionStart = Today.AddDays(7), SubscriptionEnd = Today.AddDays(97) },
            new Client { Id = 11, PassportNumber = "4512 222222", FullName = "Морозова Татьяна Львовна", Gender = Gender.Female, BirthDate = new DateOnly(1988, 10, 10), Phone = "+7 (911) 100-00-12", SubscriptionStart = Today.AddDays(14), SubscriptionEnd = Today.AddDays(104) },
        ];

        Trainers =
        [
            new Trainer { Id = 0, PassportNumber = "4601 123456", FullName = "Соколов Виктор Александрович", Gender = Gender.Male, BirthDate = new DateOnly(1990, 6, 15), Specialization = Specializations[2], ExperienceYears = 3 },
            new Trainer { Id = 1, PassportNumber = "4602 234567", FullName = "Кузнецова Наталья Сергеевна", Gender = Gender.Female, BirthDate = new DateOnly(1988, 3, 24), Specialization = Specializations[1], ExperienceYears = 5 },
            new Trainer { Id = 2, PassportNumber = "4603 345678", FullName = "Попов Максим Игоревич", Gender = Gender.Male, BirthDate = new DateOnly(1998, 11, 2), Specialization = Specializations[4], ExperienceYears = 2 },
            new Trainer { Id = 3, PassportNumber = "4604 456789", FullName = "Лебедева Дарья Александровна", Gender = Gender.Female, BirthDate = new DateOnly(1985, 7, 19), Specialization = Specializations[0], ExperienceYears = 8 },
            new Trainer { Id = 4, PassportNumber = "4605 567890", FullName = "Новиков Артур Валерьевич", Gender = Gender.Male, BirthDate = new DateOnly(1980, 1, 30), Specialization = Specializations[5], ExperienceYears = 12 },
            new Trainer { Id = 5, PassportNumber = "4606 678901", FullName = "Фёдорова Ксения Павловна", Gender = Gender.Female, BirthDate = new DateOnly(1995, 9, 8), Specialization = Specializations[6], ExperienceYears = 4 },
            new Trainer { Id = 6, PassportNumber = "4607 789012", FullName = "Морозов Георгий Львович", Gender = Gender.Male, BirthDate = new DateOnly(1982, 12, 11), Specialization = Specializations[3], ExperienceYears = 10 },
            new Trainer { Id = 7, PassportNumber = "4608 890123", FullName = "Волкова Ирина Дмитриевна", Gender = Gender.Female, BirthDate = new DateOnly(1987, 5, 27), Specialization = Specializations[7], ExperienceYears = 7 },
            new Trainer { Id = 8, PassportNumber = "4609 901234", FullName = "Зайцев Роман Олегович", Gender = Gender.Male, BirthDate = new DateOnly(2000, 2, 14), Specialization = Specializations[8], ExperienceYears = 1 },
            new Trainer { Id = 9, PassportNumber = "4610 012345", FullName = "Павлова Светлана Юрьевна", Gender = Gender.Female, BirthDate = new DateOnly(1991, 8, 3), Specialization = Specializations[0], ExperienceYears = 6 },
            new Trainer { Id = 10, PassportNumber = "4611 111111", FullName = "Семёнов Кирилл Андреевич", Gender = Gender.Male, BirthDate = new DateOnly(1979, 4, 21), Specialization = Specializations[2], ExperienceYears = 15 },
            new Trainer { Id = 11, PassportNumber = "4612 222222", FullName = "Голубева Вероника Игоревна", Gender = Gender.Female, BirthDate = new DateOnly(1993, 10, 16), Specialization = Specializations[9], ExperienceYears = 9 },
        ];

        Sessions =
        [
            // Занятия в силовом зале в текущем месяце
            new TrainingSession { Id = 0, Client = Clients[5], Trainer = Trainers[7], StartsAt = At(currentMonth, 5, 10), HallName = "Силовой зал", IsTrial = false },
            new TrainingSession { Id = 1, Client = Clients[6], Trainer = Trainers[4], StartsAt = At(currentMonth, 12, 12), HallName = "Силовой зал", IsTrial = false },
            new TrainingSession { Id = 2, Client = Clients[7], Trainer = Trainers[10], StartsAt = At(currentMonth, 20, 18), HallName = "Силовой зал", IsTrial = true },
            // Занятия в силовом зале в других месяцах
            new TrainingSession { Id = 3, Client = Clients[5], Trainer = Trainers[7], StartsAt = At(previousMonth, 5, 10), HallName = "Силовой зал", IsTrial = false },
            new TrainingSession { Id = 4, Client = Clients[5], Trainer = Trainers[0], StartsAt = At(nextMonth, 5, 10), HallName = "Силовой зал", IsTrial = false },
            // Занятие, идущее в зафиксированный момент, и занятие, окончившееся 10 минут назад;
            // для них время окончания известно, у остальных занятий оно не проставлено
            new TrainingSession { Id = 5, Client = Clients[9], Trainer = Trainers[7], StartsAt = Now.AddMinutes(-30), EndsAt = Now.AddMinutes(30), HallName = "Зал групповых занятий", IsTrial = false },
            new TrainingSession { Id = 6, Client = Clients[0], Trainer = Trainers[6], StartsAt = Now.AddMinutes(-70), EndsAt = Now.AddMinutes(-10), HallName = "Зал единоборств", IsTrial = false },
            // Остальные занятия — для статистики популярности тренеров
            new TrainingSession { Id = 7, Client = Clients[6], Trainer = Trainers[7], StartsAt = At(currentMonth, 8, 10), HallName = "Бассейн", IsTrial = false },
            new TrainingSession { Id = 8, Client = Clients[9], Trainer = Trainers[4], StartsAt = At(currentMonth, 14, 19), HallName = "Зал единоборств", IsTrial = false },
            new TrainingSession { Id = 9, Client = Clients[8], Trainer = Trainers[10], StartsAt = At(previousMonth, 15, 8), HallName = "Бассейн", IsTrial = false },
            new TrainingSession { Id = 10, Client = Clients[5], Trainer = Trainers[1], StartsAt = At(currentMonth, 22, 9), HallName = "Зал групповых занятий", IsTrial = true },
            new TrainingSession { Id = 11, Client = Clients[6], Trainer = Trainers[7], StartsAt = At(previousMonth, 18, 17), HallName = "Силовой зал", IsTrial = false },
            new TrainingSession { Id = 12, Client = Clients[7], Trainer = Trainers[4], StartsAt = At(nextMonth, 10, 11), HallName = "Кардио-зал", IsTrial = true },
            new TrainingSession { Id = 13, Client = Clients[11], Trainer = Trainers[10], StartsAt = At(nextMonth, 3, 7), HallName = "Бассейн", IsTrial = true },
            new TrainingSession { Id = 14, Client = Clients[1], Trainer = Trainers[1], StartsAt = At(previousMonth, 22, 20), HallName = "Зал единоборств", IsTrial = false },
            new TrainingSession { Id = 15, Client = Clients[2], Trainer = Trainers[6], StartsAt = At(previousMonth, 28, 18), HallName = "Зал групповых занятий", IsTrial = false },
            new TrainingSession { Id = 16, Client = Clients[8], Trainer = Trainers[7], StartsAt = At(currentMonth, 25, 8), HallName = "Бассейн", IsTrial = false },
            new TrainingSession { Id = 17, Client = Clients[0], Trainer = Trainers[4], StartsAt = At(previousMonth, 10, 19), HallName = "Кардио-зал", IsTrial = false },
            new TrainingSession { Id = 18, Client = Clients[5], Trainer = Trainers[10], StartsAt = At(currentMonth, 9, 21), HallName = "Зал единоборств", IsTrial = false },
            new TrainingSession { Id = 19, Client = Clients[9], Trainer = Trainers[1], StartsAt = At(currentMonth, 16, 7), HallName = "Кардио-зал", IsTrial = false },
            new TrainingSession { Id = 20, Client = Clients[7], Trainer = Trainers[8], StartsAt = At(previousMonth, 3, 18), HallName = "Бассейн", IsTrial = true },
            new TrainingSession { Id = 21, Client = Clients[6], Trainer = Trainers[4], StartsAt = At(nextMonth, 14, 18), HallName = "Зал единоборств", IsTrial = false },
            new TrainingSession { Id = 22, Client = Clients[1], Trainer = Trainers[2], StartsAt = At(previousMonth, 8, 8), HallName = "Кардио-зал", IsTrial = true },
            new TrainingSession { Id = 23, Client = Clients[8], Trainer = Trainers[9], StartsAt = At(currentMonth, 17, 20), HallName = "Бассейн", IsTrial = false },
            new TrainingSession { Id = 24, Client = Clients[9], Trainer = Trainers[3], StartsAt = At(nextMonth, 21, 10), HallName = "Зал групповых занятий", IsTrial = true },
            new TrainingSession { Id = 25, Client = Clients[5], Trainer = Trainers[11], StartsAt = At(currentMonth, 27, 8), HallName = "Кардио-зал", IsTrial = false },
        ];
    }
}
