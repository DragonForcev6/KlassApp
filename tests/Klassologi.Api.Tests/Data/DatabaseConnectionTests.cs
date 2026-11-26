using System.Net;
using System.Net.Http.Json;
using Dapper;
using Klassologi.Api.Features.Health;
using Klassologi.Api.Tests.Infrastructure;
using MySqlConnector;

namespace Klassologi.Api.Tests.Data;

public sealed class DatabaseConnectionTests(MySqlTestServer server) : IClassFixture<MySqlTestServer>
{
    [Fact]
    public async Task Health_svarar_ok_när_databasen_går_att_nå()
    {
        var client = server.Factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<HealthEndpoints.HealthResponse>();
        Assert.Equal(new HealthEndpoints.HealthResponse("ok", "ok"), body);
    }

    [Fact]
    public async Task Migreringarna_körs_vid_uppstart_och_registreras()
    {
        server.Factory.CreateClient(); // startar API:t, som kör migreringarna

        await using var connection = new MySqlConnection(server.ConnectionString);
        var scripts = (await connection.QueryAsync<string>("SELECT ScriptName FROM schemaversions")).ToList();

        Assert.Contains(scripts, name => name.EndsWith("0001_database_defaults.sql", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Databasen_använder_utf8mb4_strikt_läge_och_UTC()
    {
        server.Factory.CreateClient();

        await using var connection = new MySqlConnection(server.ConnectionString);
        var settings = await connection.QuerySingleAsync<(string Charset, string SqlMode, string TimeZone)>(
            """
            SELECT DEFAULT_CHARACTER_SET_NAME, @@SESSION.sql_mode, @@GLOBAL.time_zone
            FROM information_schema.SCHEMATA
            WHERE SCHEMA_NAME = DATABASE()
            """);

        Assert.Equal("utf8mb4", settings.Charset);
        Assert.Contains("STRICT_TRANS_TABLES", settings.SqlMode);
        Assert.Equal("+00:00", settings.TimeZone);
    }

    [Fact]
    public async Task Migreringarna_kan_köras_igen_utan_att_något_körs_dubbelt()
    {
        server.Factory.CreateClient();

        Klassologi.Api.Data.Migrations.Run(server.ConnectionString);

        await using var connection = new MySqlConnection(server.ConnectionString);
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM schemaversions WHERE ScriptName LIKE '%0001_database_defaults.sql'");
        Assert.Equal(1, count);
    }
}
