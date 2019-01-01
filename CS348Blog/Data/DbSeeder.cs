using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Data
{
    public static class DbSeeder
    {
        private static readonly string _password = "Password123!";

        public static void SeedDb(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            SeedUser(userManager, "Member1@email.com", "Admin");
            SeedUser(userManager, "Customer1@email.com", "StandardUser");
            SeedUser(userManager, "Customer2@email.com", "StandardUser");
            SeedUser(userManager, "Customer3@email.com", "StandardUser");
            SeedUser(userManager, "Customer4@email.com", "StandardUser");
            SeedUser(userManager, "Customer5@email.com", "StandardUser");
        }

        public static void SeedUser(UserManager<IdentityUser> userManager, string email, string role)
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = email,
                Email = email
            };
            userManager.CreateAsync(user, _password).Wait();
            userManager.AddToRoleAsync(user, role).Wait();
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            SeedRole(roleManager, "Admin");
            SeedRole(roleManager, "StandardUser");
        }

        public static void SeedRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = role
                };
                roleManager.CreateAsync(identityRole).Wait();
            }
        }
    }
}
