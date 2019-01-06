using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSC348Blog.Data
{
    //To seed users and roles to database if not exsits
    public static class DbSeeder
    {
        private static readonly string _password = "Password123!";

        //seed users and roles to database if not exsits
        public static void SeedDb(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedRoleClaims(roleManager);
            SeedUsers(userManager, roleManager);
        }

        //seed users to database if not exsits
        public static void SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            AddUser(userManager, roleManager, "Member1@email.com", "Admin").Wait();
            AddUser(userManager, roleManager, "Customer1@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer2@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer3@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer4@email.com", "StandardUser").Wait();
            AddUser(userManager, roleManager, "Customer5@email.com", "StandardUser").Wait();
        }

        //seed a user to database if not exsits
        public static async Task AddUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            //To check if user exsits in daabase. If not, create
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

        //seed roles to database if not exsits
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            AddRole(roleManager, "Admin").Wait();
            AddRole(roleManager, "StandardUser").Wait();
        }

        //seed a role to database if not exsits
        public static async Task AddRole(RoleManager<IdentityRole> roleManager, string role)
        {
            //To check if a role exsits in the database. If not, create
            if (!await roleManager.RoleExistsAsync(role))
            {
                IdentityRole identityRole = new IdentityRole(role);
                roleManager.CreateAsync(identityRole).Wait();
            }
        }

        //Add a claim to a user if not exsits
        public static async Task AddClaim(IdentityUser user, UserManager<IdentityUser> userManager, string claim, string value)
        {
            var storedClaims = await userManager.GetClaimsAsync(user);
            var claimToAdd = storedClaims.FirstOrDefault(c => c.Type.Equals(claim));

            //check if the user already get the claim to add. If not, add the claim to the user
            if(claimToAdd == null)
            {
                await userManager.AddClaimAsync(user, new Claim(claim, value));
            }
        }

        //Add a claim to a role if not exsits
        public static async Task AddClaim(IdentityRole role, RoleManager<IdentityRole> roleManager, string claim, string value)
        {
            var storedClaims = await roleManager.GetClaimsAsync(role);
            var claimToAdd = storedClaims.FirstOrDefault(c => c.Type.Equals(claim));

            //check if the role already get the claim to add. If not, add the claim to the role
            if (claimToAdd == null)
            {
                await roleManager.AddClaimAsync(role, new Claim(claim, value));
            }
        }

        //Seed roles and claims of that role to the database
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
            AddRoleClaim(roleManager, "StandardUser", "View Comment", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Like", "allowed").Wait();
            AddRoleClaim(roleManager, "StandardUser", "Dislike", "allowed").Wait();
        }

        //Add a claim to a role if not exsits
        public static async Task AddRoleClaim(RoleManager<IdentityRole> roleManager, string role, string claim, string value)
        {
            var roleToCheck = await roleManager.FindByNameAsync(role);

            //if the role does not exsits in database, add the role
            if(roleToCheck == null)
            {
                await AddRole(roleManager, role);
            }

            await AddClaim(roleToCheck, roleManager, claim, value);
        }

        //Add a user to a role if the user does not get that role
        public static async Task AddUserToRole(IdentityUser identityUser, UserManager<IdentityUser> userManager, string role)
        {
            //if the user does not have that role, add the role to the user
            if (!await userManager.IsInRoleAsync(identityUser, role))
            {
                await userManager.AddToRoleAsync(identityUser, role);
            }
        }

        //Add claims of a role to a user if the user does not get that claim
        public static async Task SeedClaims(IdentityUser identityUser, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var roleClaims = await roleManager.GetClaimsAsync(role);

            //if the user does not get a claim in that role, add that claim to the user
            foreach(var elem in roleClaims)
            {
                await AddClaim(identityUser, userManager, elem.Type, elem.Value);
            }
        }
    }
}
