using Marten;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EventSourcing.Marten_Wolverine.Testing;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public readonly MartenFixture Marten = new();

    public async Task InitializeAsync() => await Marten.InitializeAsync();

    public new async Task DisposeAsync()
    {
        await Marten.DisposeAsync();
        var store = Server.Services.GetRequiredService<IDocumentStore>();
        await store.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Postgres", Marten.PostgreSqlContainer.GetConnectionString());
    }

    public async Task<TestScope> Start()
    {
        var requiredService = Server.Services.GetRequiredService<IDocumentStore>();
        await requiredService.Advanced.Clean.DeleteAllDocumentsAsync();
        await requiredService.Advanced.Clean.DeleteAllEventDataAsync();
        return new TestScope(requiredService);
    }
}