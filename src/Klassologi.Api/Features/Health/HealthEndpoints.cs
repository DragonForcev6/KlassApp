using System.Data.Common;
using Dapper;
using MySqlConnector;

namespace Klassologi.Api.Features.Health;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", CheckHealth);
    }

    /// <summary>
    /// Svarar 200 om databasen går att fråga, annars 503. Felorsaken loggas
    /// men skickas inte till klienten.
    /// </summary>
    private static async Task<IResult> CheckHealth(MySqlDataSource dataSource, ILoggerFactory loggerFactory)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync();
            await connection.ExecuteScalarAsync<int>("SELECT 1");
            return Results.Ok(new HealthResponse("ok", "ok"));
        }
        catch (DbException ex)
        {
            loggerFactory.CreateLogger(typeof(HealthEndpoints)).LogError(ex, "Hälsokontrollen nådde inte databasen.");
            return Results.Json(new HealthResponse("unhealthy", "unreachable"), statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }

    public sealed record HealthResponse(string Status, string Database);
}
