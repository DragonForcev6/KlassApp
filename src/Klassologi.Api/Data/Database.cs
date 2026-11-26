namespace Klassologi.Api.Data;

public static class Database
{
    public const string ConnectionStringName = "Klassologi";

    public static string GetConnectionString(IConfiguration configuration) =>
        configuration.GetConnectionString(ConnectionStringName)
        ?? throw new InvalidOperationException(
            $"Anslutningssträngen 'ConnectionStrings:{ConnectionStringName}' saknas. " +
            "Sätt den med 'dotnet user-secrets' eller miljövariabeln ConnectionStrings__Klassologi. " +
            "Se Specifications/Technology.md, avsnittet Lokal utvecklingsmiljö.");
}
