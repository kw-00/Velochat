using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Options;
using Npgsql;
using Velochat.Backend.App.API.Auth;
using Velochat.Backend.App.API.Domains.Friendship;
using Velochat.Backend.App.API.Auth;
using Velochat.Backend.App.API.Domains.Messaging;
using Velochat.Backend.App.API.Domains.Rooms;
using Velochat.Backend.App.API.Realtime;
using Velochat.Backend.App.API.Realtime.Channels;
using Velochat.Backend.App.API.Realtime.RPCManagement;
using Velochat.Backend.App.Infrastructure.Persistence;
using Velochat.Backend.App.Infrastructure.Services;
using Velochat.Backend.App.Shared.Exceptions;
using Velochat.Backend.App.Shared.Metrics;
using Velochat.Backend.App.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Ope.API.Domains.at https://aka.ms/aspnet/openapi
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
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomPresenceRepository, RoomPresenceRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IRefreshTokenStateRepository, RefreshTokenStateRepository>();

// Services
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Orchestration
builder.Services.AddScoped<IAuthOrchestration, AuthOrchestration>();

// RPC channels
builder.Services.AddSingleton<RoomFeedChannels>();
builder.Services.AddSingleton<UserNotificationChannels>();

// RPC
builder.Services.AddScoped<IMessagingCommands, MessagingCommands>();
builder.Services.AddScoped<MessagingCommandDispatcher>();
builder.Services.AddSingleton<RoomFocusCache>();

builder.Services.AddScoped<IRoomsCommands, RoomsCommands>();
builder.Services.AddScoped<RoomsCommandDispatcher>();

builder.Services.AddScoped<IFriendshipCommands, FriendshipCommands>();
builder.Services.AddScoped<FriendshipCommandDispatcher>();


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


app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var error = context.Features.Get<IExceptionHandlerFeature>()!.Error;
        context.Response.StatusCode = error switch
        {
            StatusCodeException scex => scex.StatusCode,
            _ => 500
        };
        await context.Response.WriteAsync(error.Message);
    });
});

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<GlobalHub>("/hub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.Run();

