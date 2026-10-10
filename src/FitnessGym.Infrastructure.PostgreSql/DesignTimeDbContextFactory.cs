using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FitnessGym.Infrastructure.PostgreSql;

/// <summary>
/// Фабрика контекста для инструментов дизайна (dotnet ef).
/// Строка подключения используется только на этапе проектирования;
/// при запуске приложения контекст настраивается интеграцией Aspire.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FitnessGymDbContext>
{
    /// <inheritdoc/>
    public FitnessGymDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<FitnessGymDbContext>()
            .UseNpgsql("Host=localhost;Database=fitnessgym;Username=postgres;Password=postgres")
            .Options);
}
