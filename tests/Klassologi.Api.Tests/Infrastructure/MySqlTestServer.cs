using Klassologi.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MySql;

namespace Klassologi.Api.Tests.Infrastructure;

/// <summary>
/// Startar en riktig MySQL i Docker med samma serverinställningar som
/// docker-compose.yml, och API:t mot den databasen.
/// </summary>
public sealed class MySqlTestServer : IAsyncLifetime
{
    public MySqlContainer Container { get; } = new MySqlBuilder("mysql:8.4")
        .WithCommand(
            "--character-set-server=utf8mb4",
            "--collation-server=utf8mb4_0900_ai_ci",
            "--sql-mode=STRICT_TRANS_TABLES,ONLY_FULL_GROUP_BY,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION",
            "--default-time-zone=+00:00")
        .Build();

    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.UseSetting($"ConnectionStrings:{Database.ConnectionStringName}", ConnectionString));
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await Container.DisposeAsync();
    }
}
