using Marten;

namespace EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine;

public class ProjectionFixture : PostgreSqlFixture
{
    public async Task<DocumentStore> Start(Action<StoreOptions> configure)
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
        });

        await store.Advanced.Clean.DeleteAllEventDataAsync();
        await store.Advanced.Clean.DeleteAllDocumentsAsync();

        return store;
    }
}