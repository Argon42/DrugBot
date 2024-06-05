using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrugBot.ServerApp.Data;

public class ApplicationDbInitializer
{
    private readonly ILogger<ApplicationDbInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private const string AdminEmail = "root@drugbot.com";
    private const string Password = $"Admin12345!";

    public ApplicationDbInitializer(
        ILogger<ApplicationDbInitializer> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Initialize()
    {
        await _context.Database.MigrateAsync();
        await CreateDefaultRolesAndUsers();
    }

    internal async Task CreateDefaultRolesAndUsers()
    {
        await CreateDefaultRoles(_context, _roleManager);
        await CreateDefaultUsers(_context, _userManager);
    }

    private async Task CreateDefaultUsers(ApplicationDbContext dataContext, UserManager<ApplicationUser> userManager)
    {
        if (dataContext.Users.Any())
            return;

        ApplicationUser user = new()
        {
            UserName = AdminEmail,
            Email = AdminEmail,
            NormalizedEmail = AdminEmail.ToUpper(),
            NormalizedUserName = AdminEmail.ToUpper(),
            EmailConfirmed = true,
        };
        var password = Password;
        IdentityResult result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.Admin);

            _logger.LogDebug($"Admin user created successfully. Email: {AdminEmail}, Password: {password}");
        }
    }

    private async Task CreateDefaultRoles(ApplicationDbContext dataContext, RoleManager<IdentityRole> roleManager)
    {
        if (dataContext.Roles.Any())
            return;

        foreach (string role in Roles.All)
            if (!roleManager.RoleExistsAsync(role).Result)
                await roleManager.CreateAsync(new IdentityRole(role));
    }
}