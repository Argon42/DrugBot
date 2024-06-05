using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerApp.Data;

public class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private const string AdminEmail = "admin@example.com";

    public ApplicationDbInitializer(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Initialize()
    {
        if (await _context.Database.EnsureCreatedAsync() == false)
        {
            await _context.Database.MigrateAsync();
        }

        if (_context.Database.GetPendingMigrations().Any())
        {
            await _context.Database.MigrateAsync();
        }

        await CreateDefaultRolesAndUsers();
    }

    private static string CreatePassword() => $"P1!{Guid.NewGuid()}";

    internal async Task CreateDefaultRolesAndUsers()
    {
        await CreateDefaultRoles(_context, _roleManager);
        await CreateDefaultUsers(_context, _userManager);
    }

    private async Task CreateDefaultUsers(ApplicationDbContext dataContext, UserManager<IdentityUser> userManager)
    {
        if (dataContext.Users.Any())
            return;

        IdentityUser user = new()
        {
            UserName = AdminEmail,
            Email = AdminEmail,
            NormalizedEmail = AdminEmail.ToUpper(),
            NormalizedUserName = AdminEmail.ToUpper()
        };
        var password = CreatePassword();
        IdentityResult result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.Admin);

            Console.WriteLine($"Admin user created successfully. Email: {AdminEmail}, Password: {password}");
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