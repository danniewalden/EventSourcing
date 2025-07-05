using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine;

public class PostgreSqlFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer { get; } = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        try
        {
            await PostgreSqlContainer.StartAsync();
        }
        catch (Exception ex)
        {
            if (!await IsContainerRunning())
            {
                throw new InvalidOperationException("Failed to start PostgreSQL container and it is not running.", ex);
            }
        }
    }

    public async Task DisposeAsync()
    {
        try
        {
            await PostgreSqlContainer.StopAsync();
        }
        catch (Exception ex)
        {
            if (await IsContainerRunning())
            {
                throw new InvalidOperationException("Failed to stop PostgreSQL container and it is still running.", ex);
            }
        }
    }

    private async Task<bool> IsContainerRunning()
    {
        try
        {
            await using var connection = new NpgsqlConnection(PostgreSqlContainer.GetConnectionString());
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}