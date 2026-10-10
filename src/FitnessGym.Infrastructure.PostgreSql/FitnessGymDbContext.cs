using FitnessGym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessGym.Infrastructure.PostgreSql;

/// <summary>
/// Контекст базы данных фитнес-клуба.
/// Наследование Client и Trainer от Person отображается по стратегии table-per-type:
/// у клиентов и тренеров совпадают диапазоны идентификаторов, поэтому общая
/// таблица с дискриминатором (TPH) им не подходит.
/// </summary>
/// <param name="options">Параметры контекста.</param>
public class FitnessGymDbContext(DbContextOptions<FitnessGymDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Клиенты фитнес-клуба.
    /// </summary>
    public DbSet<Client> Clients => Set<Client>();

    /// <summary>
    /// Тренеры фитнес-клуба.
    /// </summary>
    public DbSet<Trainer> Trainers => Set<Trainer>();

    /// <summary>
    /// Справочник специализаций тренеров.
    /// </summary>
    public DbSet<Specialization> Specializations => Set<Specialization>();

    /// <summary>
    /// Записи клиентов на персональные занятия.
    /// </summary>
    public DbSet<TrainingSession> Sessions => Set<TrainingSession>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.UseTptMappingStrategy();
            entity.ToTable("persons");
            entity.Property(p => p.Id).ValueGeneratedNever();
            entity.Property(p => p.PassportNumber).IsRequired();
            entity.Property(p => p.FullName).IsRequired();
        });

        modelBuilder.Entity<Client>().ToTable("clients");

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.ToTable("trainers");
            entity.HasOne(t => t.Specialization)
                .WithMany()
                .HasForeignKey("SpecializationId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.ToTable("specializations");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedNever();
            entity.Property(s => s.Name).IsRequired();
            entity.HasData(
                new Specialization { Id = 0, Name = "Йога" },
                new Specialization { Id = 1, Name = "Пилатес" },
                new Specialization { Id = 2, Name = "Кроссфит" },
                new Specialization { Id = 3, Name = "Плавание" },
                new Specialization { Id = 4, Name = "Бокс" },
                new Specialization { Id = 5, Name = "Тяжёлая атлетика" },
                new Specialization { Id = 6, Name = "Стретчинг" },
                new Specialization { Id = 7, Name = "Функциональный тренинг" },
                new Specialization { Id = 8, Name = "Лёгкая атлетика" },
                new Specialization { Id = 9, Name = "Реабилитация" });
        });

        modelBuilder.Entity<TrainingSession>(entity =>
        {
            entity.ToTable("sessions");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedNever();
            entity.Property(s => s.HallName).IsRequired();
            if (Database.IsNpgsql())
            {
                // Занятия проходят по местному времени клуба: тип без часового пояса
                entity.Property(s => s.StartsAt).HasColumnType("timestamp without time zone");
                entity.Property(s => s.EndsAt).HasColumnType("timestamp without time zone");
            }
            entity.HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(s => s.Trainer)
                .WithMany()
                .HasForeignKey("TrainerId")
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
