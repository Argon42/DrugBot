using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders;
using DrugBot.DataBase.DataProviders.Interfaces;
using DrugBot.DataBase.Initializers;
using DrugBot.ServerApp.Components;
using DrugBot.ServerApp.Components.Account;
using DrugBot.ServerApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrugBot.ServerApp;

public static class ConfigurationExtension
{
    public static IServiceCollection AddIdentityAndAuthentication(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        return services;
    }

    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static IServiceCollection AddIdentityDb(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }

    public static void ConfigurateHttpRequestPipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
    }

    public static void ConfigurateComponents(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();
    }

    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddTransient<ApplicationDbInitializer>();
        
        services.AddTransient<MagicBallDbInitializer>();
        services.AddTransient<ChineseDbInitializer>();
        services.AddTransient<EmojiDbInitializer>();
        services.AddTransient<PredictionDbInitializer>();
        services.AddTransient<WisdomDbInitializer>();
        
        services.AddSingleton<IMagicBallDataProvider, MagicBallDataProvider>();
        services.AddSingleton<IChineseDataProvider, ChineseDataProvider>();
        services.AddSingleton<IEmojiDataProvider, EmojiDataProvider>();
        services.AddSingleton<IPredictionDataProvider, PredictionDataProvider>();
        services.AddSingleton<IWisdomDataProvider, WisdomDataProvider>();
        return services;
    }
    
    public static IServiceCollection AddMagicBallDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MagicBallConnection") ??
                               throw new InvalidOperationException("Connection string 'MagicBallConnection' not found.");
        services.AddDbContext<MagicBallDbContext>(options =>
            options.UseSqlite(connectionString), contextLifetime: ServiceLifetime.Singleton);
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
    
    public static IServiceCollection AddChineseDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ChineseConnection") ??
                               throw new InvalidOperationException("Connection string 'ChineseConnection' not found.");
        services.AddDbContext<ChineseDbContext>(options =>
            options.UseSqlite(connectionString), contextLifetime: ServiceLifetime.Singleton);
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
    
    public static IServiceCollection AddEmojiDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EmojiConnection") ??
                               throw new InvalidOperationException("Connection string 'EmojiConnection' not found.");
        services.AddDbContext<EmojiDbContext>(options =>
            options.UseSqlite(connectionString), contextLifetime: ServiceLifetime.Singleton);
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
    
    public static IServiceCollection AddPredictionDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PredictionConnection") ??
                               throw new InvalidOperationException("Connection string 'PredictionConnection' not found.");
        services.AddDbContext<PredictionDbContext>(options =>
            options.UseSqlite(connectionString), contextLifetime: ServiceLifetime.Singleton);
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
    
    public static IServiceCollection AddWisdomDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WisdomConnection") ??
                               throw new InvalidOperationException("Connection string 'WisdomConnection' not found.");
        services.AddDbContext<WisdomDbContext>(options =>
            options.UseSqlite(connectionString), contextLifetime: ServiceLifetime.Singleton);
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
}