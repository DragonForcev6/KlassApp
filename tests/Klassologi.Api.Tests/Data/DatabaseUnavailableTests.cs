using System.Net;
using System.Net.Http.Json;
using Klassologi.Api.Features.Health;
using Klassologi.Api.Tests.Infrastructure;

namespace Klassologi.Api.Tests.Data;

/// <summary>
/// Egen databasserver, eftersom testet stoppar den.
/// </summary>
public sealed class DatabaseUnavailableTests(MySqlTestServer server) : IClassFixture<MySqlTestServer>
{
    [Fact]
    public async Task Health_svarar_503_utan_feldetaljer_när_databasen_har_försvunnit()
    {
        var client = server.Factory.CreateClient(); // API:t startar och migrerar mot en fungerande databas
        await server.Container.StopAsync();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<HealthEndpoints.HealthResponse>();
        Assert.Equal(new HealthEndpoints.HealthResponse("unhealthy", "unreachable"), body);
    }
}
