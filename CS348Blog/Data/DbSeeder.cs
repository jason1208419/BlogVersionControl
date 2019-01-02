using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSC348Blog.Data
{
    public static class DbSeeder
    {
        private static readonly string _password = "Password123!";

        public static void SeedDb(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedRoleClaims(roleManager);
            SeedUsers(userManager, roleManager);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            AddUser(userManager, roleManager, "Member1@email.com", "Admin").Wait();
            AddUser(userManager, roleManager, "Customer1@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer2@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer3@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer4@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer5@email.com", "StandardUser").Wait();
        }

        public static async Task AddUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                IdentityUser identityUser = new IdentityUser()
                {
                    UserName = email,
                    Email = email
                };
                await userManager.CreateAsync(identityUser, _password);
                await AddUserToRole(identityUser, userManager, role);
                await SeedClaims(identityUser, roleManager, userManager, role);
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            AddRole(roleManager, "Admin").Wait();
            AddRole(roleManager, "StandardUser").Wait();
        }

        public static async Task AddRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                IdentityRole identityRole = new IdentityRole(role);
                roleManager.CreateAsync(identityRole).Wait();
            }
        }

        public static async Task AddClaim(IdentityUser user, UserManager<IdentityUser> userManager, string claim, string value)
        {
            var storedClaims = await userManager.GetClaimsAsync(user);
            var claimToCheck = storedClaims.FirstOrDefault(c => c.Type == claim);

            if(claimToCheck == null)
            {
                await userManager.AddClaimAsync(user, new Claim(claim, value));
            }
        }

        public static async Task AddClaim(IdentityRole role, RoleManager<IdentityRole> roleManager, string claim, string value)
        {
            var storedClaims = await roleManager.GetClaimsAsync(role);
            var claimToCheck = storedClaims.FirstOrDefault(c => c.Type == claim);

            if (claimToCheck == null)
            {
                await roleManager.AddClaimAsync(role, new Claim(claim, value));
            }
        }

        public static void SeedRoleClaims(RoleManager<IdentityRole> roleManager)
        {
            AddRoleClaim(roleManager, "Admin", "Create Post", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Edit Post", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Delete Post", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "View Post", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "View Post List", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Create Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Edit Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Delete Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "View Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Like", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Dislike", "allowed").Wait();
            AddRoleClaim(roleManager, "Admin", "Permission Panel", "allowed").Wait();

            AddRoleClaim(roleManager, "StandardUser", "View Post", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "View Post List", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Create Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Edit Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Delete Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "View Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Like", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Dislike", "allowed").Wait();
        }

        public static async Task AddRoleClaim(RoleManager<IdentityRole> roleManager, string role, string claim, string value)
        {
            var roleToCheck = await roleManager.FindByNameAsync(role);
            if(roleToCheck == null)
            {
                await AddRole(roleManager, role);
            }

            await AddClaim(roleToCheck, roleManager, claim, value);
        }

        public static async Task AddUserToRole(IdentityUser identityUser, UserManager<IdentityUser> userManager, string role)
        {
            if (!await userManager.IsInRoleAsync(identityUser, role))
            {
                await userManager.AddToRoleAsync(identityUser, role);
            }
        }

        public static async Task SeedClaims(IdentityUser identityUser, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var storedClaims = await roleManager.GetClaimsAsync(role);
            foreach(var elem in storedClaims)
            {
                await AddClaim(identityUser, userManager, elem.Type, elem.Value);
            }
        }
    }
}
