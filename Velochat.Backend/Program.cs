using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Options;
using Npgsql;
using Velochat.Backend.App.Layers.Domains.User;
using Velochat.Backend.App.Layers.Hubs;
using Velochat.Backend.App.Layers.Infrastructure;
using Velochat.Backend.App.Shared.Auth;
using Velochat.Backend.App.Shared.Metrics;
using Velochat.Backend.App.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("Db"));
builder.Services.Configure<ChatOptions>(builder.Configuration.GetSection("Chat"));
builder.Services.AddSingleton(sp =>
{
    var connectionString = sp
        .GetRequiredService<IOptions<DbOptions>>()
            .Value
            .ConnectionString;
    return NpgsqlDataSource.Create(connectionString);
});

// Repositories
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenStateRepository, RefreshTokenStateRepository>();
builder.Services.AddScoped<IRoomPresenceRepository, RoomPresenceRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
// Services
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Orchestration
builder.Services.AddScoped<IUserOrchestration, UserOrchestration>();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services
    .AddAuthentication("AccessToken")
    .AddScheme<AuthenticationSchemeOptions, AccessTokenAuthHandler>("AccessToken", null);

// Telemetry
builder.Services.AddMetrics(conf =>
{
    conf.EnableMetrics(VelochatMetrics.MetricsName);
});

var app = builder.Build();

app.MapControllers();
app.MapHub<GlobalHub>("/hub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

