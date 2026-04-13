using Microsoft.Extensions.Options;
using Npgsql;
using Velochat.Backend.App.Layers.Domains.Identity;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("Db"));
builder.Services.AddSingleton(sp =>
{
    var connectionString = sp
        .GetRequiredService<IOptions<DbOptions>>()
        .Value.ConnectionString;
    return new NpgsqlConnection(connectionString);
});

// Repositories

// Services
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();

// Orchestration
builder.Services.AddScoped<IIdentityOrchestration, IdentityOrchestration>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

