var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("fitnessgym");

builder.AddProject<Projects.FitnessGym_Api>("api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
