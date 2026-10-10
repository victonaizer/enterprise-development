using System.Text.Json.Serialization;
using FitnessGym.Application.Services;
using FitnessGym.Contracts.Interfaces;
using FitnessGym.Domain.Interfaces;
using FitnessGym.Domain.Seeds;
using FitnessGym.Infrastructure.InMemory;

var builder = WebApplication.CreateBuilder(args);

var data = ClubDataSeeder.Create();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IClientRepository>(_ => new InMemoryClientRepository(data.Clients));
builder.Services.AddSingleton<ITrainerRepository>(_ => new InMemoryTrainerRepository(data.Trainers));
builder.Services.AddSingleton<ISpecializationRepository>(_ => new InMemorySpecializationRepository(data.Specializations));
builder.Services.AddSingleton<ITrainingSessionRepository>(_ => new InMemoryTrainingSessionRepository(data.Sessions));
builder.Services.AddSingleton<IClubService, ClubService>();

var app = builder.Build();

app.MapOpenApi();
app.MapControllers();

app.Run();
