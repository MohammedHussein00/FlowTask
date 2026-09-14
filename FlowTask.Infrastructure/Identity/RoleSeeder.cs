namespace FlowTask.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using FlowTask.Domain.Identity;

public interface IRoleSeeder
{
    Task SeedAsync();
}

public class RoleSeeder : IRoleSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public RoleSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        string[] roles = ["SuperAdmin", "Admin", "Member", "Guest"];
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new AppRole { Name = role });
        }

        var adminEmail = "admin@flowtask.io";
        if (await _userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(admin, "Admin@FlowTask1!");
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, "SuperAdmin");
        }
    }
}