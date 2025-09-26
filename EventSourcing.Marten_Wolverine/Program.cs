using EventSourcing.Marten_Wolverine;
using JasperFx;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using Marten;
using Marten.Events.Daemon.Internals;
using Marten.Exceptions;
using Newtonsoft.Json;
using Npgsql;
using Polly;
using Polly.Retry;
using Scalar.AspNetCore;
using Weasel.Core;
using WebApplication1;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMarten(v =>
    {
        var options = new StoreOptions();
        options.Connection(builder.Configuration.GetConnectionString("Postgres")!);
        options.UseNewtonsoftForSerialization(
            EnumStorage.AsString,
            Casing.CamelCase,
            CollectionStorage.AsArray,
            NonPublicMembersStorage.All,
            settings =>
            {
                settings.TypeNameHandling = TypeNameHandling.Auto;
                settings.Formatting = Formatting.Indented;
            });

        options.ApplyAllConfigurations(v);

        options.Events.DatabaseSchemaName = "eventstore";
        options.DatabaseSchemaName = "database";
        options.Events.UseArchivedStreamPartitioning = true;
        options.Events.UseIdentityMapForAggregates = true;
        options.ConfigurePolly(p =>
        {
            p.AddRetry(new RetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder()
                    .Handle<NpgsqlException>()
                    .Handle<MartenCommandException>()
                    .Handle<EventLoaderException>(),
                Delay = TimeSpan.FromMilliseconds(50),
                MaxRetryAttempts = 3,
            });
        });
        options.DisableNpgsqlLogging = true;
        options.Events.AppendMode = EventAppendMode.Quick;

        return options;
    })
    // .UseNpgsqlDataSource("marten")
    .IntegrateWithWolverine(integration => { integration.UseFastEventForwarding = true; })
    .UseLightweightSessions()
    .AddAsyncDaemon(DaemonMode.Solo);

// Wolverine usage is required for WolverineFx.Http
builder.Host.UseWolverine(opts =>
{
    // This middleware will apply to the HTTP
    // endpoints as well
    opts.Policies.AutoApplyTransactions();

    // Setting up the outbox on all locally handled
    // background tasks
    opts.Policies.UseDurableLocalQueues();

    opts.ApplicationAssembly = typeof(Program).Assembly;

    opts.UseFluentValidation();
});

builder.Services.AddWolverineHttp();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapWolverineEndpoints(options => { options.UseFluentValidationProblemDetailMiddleware(); });

app.MapScalarApiReference(options => { options.Theme = ScalarTheme.Kepler; });

return await app.RunJasperFxCommands(args);