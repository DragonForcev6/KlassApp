using DbUp;

namespace Klassologi.Api.Data;

/// <summary>
/// Kör versionerade SQL-skript i Data/Migrations/ med DbUp. Skripten bäddas in i
/// assemblyn och körs i namnordning. Redan körda skript registreras i tabellen
/// SchemaVersions och körs inte igen.
/// </summary>
public static class Migrations
{
    public static void Run(string connectionString)
    {
        var upgrader = DeployChanges.To
            .MySqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                typeof(Migrations).Assembly,
                name => name.Contains(".Data.Migrations.", StringComparison.Ordinal))
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new InvalidOperationException(
                $"Databasmigreringen misslyckades i skriptet '{result.ErrorScript?.Name}'.",
                result.Error);
        }
    }
}
