using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;


var builder = DistributedApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

// Add projects
var api = builder.AddProject<EventSourcing_Marten_Wolverine>("eventsourcing-examples");


var postgres = builder.AddPostgres("postgres")
        .WithDataVolume("eventsourcing-examples-data")
        .WithPgAdmin(resourceBuilder => resourceBuilder.WithHostPort(45119))
    ;
var postgresdb = postgres.AddDatabase("eventsourcing-examples-database");

// Reference PostgreSQL in your API
api.WithReference(postgresdb, "Postgres").WaitFor(postgresdb);

await builder.Build().RunAsync();