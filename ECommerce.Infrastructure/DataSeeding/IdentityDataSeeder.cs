using ECommerce.Application.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.DataSeeding
{
    internal class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> _logger;

        public IdentityDataSeeder(
            StoreIdentityDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<IdentityDataSeeder> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync(ct);
                }


                if ((!await _roleManager.Roles.AnyAsync(ct)))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admim"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmim"));
                }

                if ((!await _userManager.Users.AnyAsync(ct)))
                {
                    var superAdmin = new ApplicationUser()
                    {
                        DisplayName = "Super Admin User",
                        Email = "SuperAdmin123@gmail.com",
                        UserName = "SuperAdmin123",
                        PhoneNumber = "01349283992"
                    };

                    var createResult = await _userManager.CreateAsync(superAdmin, "P@ssword");
                    if (createResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    }
                    else
                    {
                        var ErrorsDescriptions = string.Join(';', createResult.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Can Not Seed Default Admin {ErrorsDescriptions}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Identity Data Seeding Failed");
            }
        }
    }
}
