using System.Text.Json.Serialization;
using FitnessGym.Api.Services;
using FitnessGym.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IClubService>(_ => new ClubService(ClubDataSeeder.Create()));

var app = builder.Build();

app.MapOpenApi();
app.MapControllers();

app.Run();
