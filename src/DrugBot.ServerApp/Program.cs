using DrugBot.ServerApp;
using Microsoft.AspNetCore.Identity;
using DrugBot.ServerApp.Components.Account;
using DrugBot.ServerApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("app_secrets.json", true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddIdentityAndAuthentication();
builder.Services.AddDb(builder.Configuration);
builder.Services.AddIdentityDb();

builder.Services.AddProjectServices();

var app = builder.Build();

app.ConfigurateHttpRequestPipeline();
app.ConfigurateComponents();

app.Run();