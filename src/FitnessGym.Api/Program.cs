using System.Text.Json.Serialization;
using FitnessGym.Application.Services;
using FitnessGym.Contracts.Interfaces;
using FitnessGym.Domain.Interfaces;
using FitnessGym.Infrastructure.PostgreSql;
using FitnessGym.ServiceDefaults;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<FitnessGymDbContext>("fitnessgym");

builder.Services.AddScoped<IClientRepository, PostgreSqlClientRepository>();
builder.Services.AddScoped<ITrainerRepository, PostgreSqlTrainerRepository>();
builder.Services.AddScoped<ISpecializationRepository, PostgreSqlSpecializationRepository>();
builder.Services.AddScoped<ITrainingSessionRepository, PostgreSqlTrainingSessionRepository>();
builder.Services.AddScoped<IClubService, ClubService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    DbInitializer.Initialize(scope.ServiceProvider.GetRequiredService<FitnessGymDbContext>());
}

app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapControllers();

app.Run();
