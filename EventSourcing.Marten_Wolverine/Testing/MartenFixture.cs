using Marten;

namespace EventSourcing.Marten_Wolverine.Testing;

public class MartenFixture : PostgreSqlFixture
{
    public async Task<TestScope> Start(Action<StoreOptions> configure)
    {
        var store = DocumentStore.For(opts =>
        {
            configure.Invoke(opts);

            opts.Connection(PostgreSqlContainer.GetConnectionString());

            opts.Events.DatabaseSchemaName = "eventstore";
            opts.DatabaseSchemaName = "database";
            opts.Events.UseArchivedStreamPartitioning = true;
            opts.Events.UseIdentityMapForAggregates = true;
            opts.Events.MetadataConfig.HeadersEnabled = true;
            opts.Projections.UseIdentityMapForAggregates = true;
            opts.UseNewtonsoftForSerialization();
        });

        await store.Advanced.Clean.DeleteAllEventDataAsync();
        await store.Advanced.Clean.DeleteAllDocumentsAsync();

        return new TestScope(store);
    }
}

public class TestScope(IDocumentStore store) : IDisposable, IAsyncDisposable
{
    public async Task Given(Guid id, params object[] events)
    {
        await using var session = store.LightweightSession();
        session.Events.StartStream(id, events);
        await session.SaveChangesAsync();
    }

    public async Task Then(Func<IQuerySession, Task> action)
    {
        await using var session = store.QuerySession();
        await action(session);
    }
    public async Task Then(Func<Task> action)
    {
        await action();
    }

    public async Task When<T, TU>(Func<(T, TU)> decider)
    {
        await Task.CompletedTask;
    }

    public async Task When(Func<Task> when)
    {
        await when();
    }

    public async Task<HttpResponseMessage> When(Func<Task<HttpResponseMessage>> when)
    {
        return await when();
    }

    public void Dispose()
    {
        // store.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        // await store.DisposeAsync();
    }
}