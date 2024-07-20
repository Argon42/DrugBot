using DrugBot.DataBase.Initializers;
using DrugBot.Infrastructure;
using DrugBot.ServerApp;
using DrugBot.ServerApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("app_secrets.json", true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddIdentityAndAuthentication();
builder.Services.AddDb(builder.Configuration);
builder.Services.AddPredictionDb(builder.Configuration);
builder.Services.AddChineseDb(builder.Configuration);
builder.Services.AddIdentityDb();

ApplicationConfiguration.ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddProjectServices();

var app = builder.Build();

app.ConfigurateHttpRequestPipeline();
app.ConfigurateComponents();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbInitializer>().Initialize();
await app.Services.CreateScope().ServiceProvider.GetRequiredService<MagicBallDbInitializer>().Initialize();
await app.Services.CreateScope().ServiceProvider.GetRequiredService<ChineseDbInitializer>().Initialize();

app.Run();