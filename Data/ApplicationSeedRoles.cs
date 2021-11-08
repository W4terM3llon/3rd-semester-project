using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RestaurantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Data
{
    public static class ApplicationSeedRoles
    {
        public static void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            Task<IdentityResult> roleResult;

            List<string> roles = new List<string>
            {
                "RestaurantManager",
                "User"
            };

            foreach (string role in roles)
            {
                //Check that there is an  role and create if not
                Task<bool> hasRole = roleManager.RoleExistsAsync(role);
                hasRole.Wait();

                if (!hasRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new UserRole("Administrator"));
                    roleResult.Wait();
                }
            }
        }
    }
}
