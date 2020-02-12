using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public static class IdentitySeedData
    {
        //Admin Data
        private const string adminUser = "Daniel";
        private const string adminPass = "Secret@123";
        private const string adminRoleName = "Admin";

        //Manager Data
        private const string managerUser = "User";
        private const string managerPass = "Faculty@123";
        private const string managerRoleName = "Manager";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            //Role Manager declaration
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                                                        .GetRequiredService<RoleManager<IdentityRole>>();

            //Admin Role
            IdentityRole adminRole = await roleManager.FindByNameAsync(adminRoleName);

            if (adminRole == null)
            {
                adminRole = new IdentityRole(adminRoleName);
                await roleManager.CreateAsync(adminRole);
            }

            //Manager Role
            IdentityRole managerRole = await roleManager.FindByNameAsync(managerRoleName);

            if (managerRole == null)
            {
                managerRole = new IdentityRole(managerRoleName);
                await roleManager.CreateAsync(managerRole);
            }

            //User Manager Declaration
            UserManager<IdentityUser> userManager = app.ApplicationServices
                                                        .GetRequiredService<UserManager<IdentityUser>>();

            //Admin User
            IdentityUser admin = await userManager.FindByIdAsync(adminUser);

            if (admin == null)
            {
                admin = new IdentityUser(adminUser);
                await userManager.CreateAsync(admin, adminPass);
                await userManager.AddToRoleAsync(admin, adminRoleName);
            }
            else
            {
                if (!(await userManager.IsInRoleAsync(admin, adminRoleName)))
                {
                    await userManager.AddToRoleAsync(admin, adminRoleName);
                }
            }

            //Manager User
            IdentityUser manager = await userManager.FindByIdAsync(managerUser);

            if (manager == null)
            {
                manager = new IdentityUser(managerUser);
                await userManager.CreateAsync(manager, managerPass);
                await userManager.AddToRoleAsync(manager, managerRoleName);
            }
            else
            {
                if (!(await userManager.IsInRoleAsync(manager, managerRoleName)))
                {
                    await userManager.AddToRoleAsync(manager, managerRoleName);
                }
            }
        }
    }
}

