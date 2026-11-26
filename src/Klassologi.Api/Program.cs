using Klassologi.Api.Data;
using Klassologi.Api.Features.Health;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp => new MySqlDataSource(Database.GetConnectionString(sp.GetRequiredService<IConfiguration>())));

var app = builder.Build();

Migrations.Run(Database.GetConnectionString(app.Configuration));

app.MapHealthEndpoints();

app.Run();
