using FitnessGym.Domain.Seeds;
using Microsoft.EntityFrameworkCore;

namespace FitnessGym.Infrastructure.PostgreSql;

/// <summary>
/// Инициализатор базы данных: применяет миграции и выполняет первоначальное наполнение.
/// Справочник специализаций заполняется миграцией; клиенты, тренеры и записи на занятия
/// наполняются при старте — их даты вычисляются относительно текущего момента времени,
/// поэтому данные остаются пригодными при любом времени запуска приложения.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Применяет миграции и при необходимости наполняет базу тестовыми данными.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public static void Initialize(FitnessGymDbContext context)
    {
        context.Database.Migrate();

        if (context.Clients.Any() || context.Trainers.Any() || context.Sessions.Any())
        {
            return;
        }

        var data = ClubDataSeeder.Create(DateTime.Now);

        // Специализации уже добавлены миграцией: навигации тренеров
        // перепривязываются к существующим строкам справочника,
        // чтобы EF не пытался вставить их повторно.
        var specializations = context.Specializations.ToDictionary(s => s.Id);
        foreach (var trainer in data.Trainers)
        {
            trainer.Specialization = specializations[trainer.Specialization.Id];
        }

        context.Clients.AddRange(data.Clients);
        context.Trainers.AddRange(data.Trainers);
        context.Sessions.AddRange(data.Sessions);
        context.SaveChanges();
    }
}
