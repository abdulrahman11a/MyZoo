using Microsoft.AspNetCore.Identity;
using Clinic.Core.Entities.Identity;
using Clinic.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Clinic.infrastructure.Identity
{
    public static class IdentityStoreDataseed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger logger = null)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var context = scope.ServiceProvider.GetRequiredService<AppIdentityDBContext>();

                // Ensure the database is created (use Migrate for production)
                await context.Database.MigrateAsync();

                // Seed Roles
                // Seed Roles
                string[] roleNames = { "Patient", "Vet", "Admin" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        var role = new AppRole { Name = roleName }; // Using AppRole for custom role
                        await roleManager.CreateAsync(role);
                    }
                }

                // Seed Admin User
                var adminEmail = "admin@clinic.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        DisplayName = "Clinic Admin",
                        EmailConfirmed = true,
                        PhoneNumber ="01502755473"
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                        // Add address for admin
                        var adminAddress = new UserAddress
                        {
                            FirstName = "Clinic",
                            LastName = "Admin",
                            Street = "123 Admin St",
                            City = "Admin City",
                            Country = "USA",
                            AppUserId = adminUser.Id
                        };
                        context.UserAddress.Add(adminAddress);
                        await context.SaveChangesAsync();
                    }
                }

                // Seed Vet User
                var vetEmail = "vet@clinic.com";
                var vetUser = await userManager.FindByEmailAsync(vetEmail);
                if (vetUser == null)
                {
                    vetUser = new AppUser
                    {
                        UserName = vetEmail,
                        Email = vetEmail,
                        DisplayName = "Dr. Vet",
                        EmailConfirmed = true,
                      PhoneNumber ="01507735473"
                    };
                    var result = await userManager.CreateAsync(vetUser, "Vet@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(vetUser, "Vet");

                        // Add address for vet
                        var vetAddress = new UserAddress
                        {
                            FirstName = "Dr.",
                            LastName = "Vet",
                            Street = "456 Vet Ave",
                            City = "Vet City",
                            Country = "USA",
                            AppUserId = vetUser.Id
                        };
                        context.UserAddress.Add(vetAddress);
                        await context.SaveChangesAsync();
                    }
                }

                // Seed Patient User
                var patientEmail = "patient@clinic.com";
                var patientUser = await userManager.FindByEmailAsync(patientEmail);
                if (patientUser == null)
                {
                    patientUser = new AppUser
                    {
                        UserName = patientEmail,
                        Email = patientEmail,
                        DisplayName = "John Doe",
                        EmailConfirmed = true,
                        PhoneNumber ="01507755473"
                        
                    };
                    var result = await userManager.CreateAsync(patientUser, "Patient@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(patientUser, "Patient");

                        // Add address for patient
                        var patientAddress = new UserAddress
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            Street = "789 Patient Rd",
                            City = "Patient City",
                            Country = "USA",
                            AppUserId = patientUser.Id
                        };
                        context.UserAddress.Add(patientAddress);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred during the identity seeding process.");
                throw;
            }
        }
    }
}
