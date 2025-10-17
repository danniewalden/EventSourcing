using Marten;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EventSourcing.Marten_Wolverine.Testing;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public readonly MartenFixture Marten = new();

    public async Task InitializeAsync() => await Marten.InitializeAsync();

    public new async Task DisposeAsync() => await Marten.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Postgres", Marten.PostgreSqlContainer.GetConnectionString());
    }

    public TestScope Start() => new(Server.Services.GetRequiredService<IDocumentStore>());
}