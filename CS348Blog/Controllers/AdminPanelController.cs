using CSC348Blog.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    [Authorize("Permission Panel")]
    [AutoValidateAntiforgeryToken]
    /*
        This controller deal with Get request and post request about users and role
        in the admin panel
        Only admin can do all these get and post requests
    */
    public class AdminPanelController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IRepo _repo;

        //Initialize the database, sigm in manager, role manager and user manager
        //when construting the controller
        public AdminPanelController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRepo repo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _repo = repo;
        }

        //Get all users from database and turn to a list. Then display all users in a page
        public async Task<IActionResult> UserList()
        {
            var userList = await _repo.GetUserList();
            return View(userList);
        }

        //Get the requested user from database and display the details of the user
        public async Task<IActionResult> User(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            UserViewModel model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,

                AllClaims = allClaims.ToList(),
                UserClaims = userClaims.ToList(),
                AllRoles = allRoles,
                UserRoles = new List<IdentityRole>()
            };

            //loop though the role and role name of the user get
            foreach (var role in allRoles)
            {
                foreach (var userRole in userRoles)
                {
                    //if the user get that role, add to a list
                    if (role.Name.Equals(userRole))
                    {
                        model.UserRoles.Add(role);
                        break;
                    }
                }
            }

            return View(model);
        }

        //Display the page of creating user
        public IActionResult CreateUser()
        {
            return View();
        }

        //Get the user from database which the user want to edit and display details on input area
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            UserViewModel model = new UserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };

            await EditView(id, model);

            return View(model);
        }

        //setup informations need to display
        private async Task EditView(string id, UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var userClaims = await _userManager.GetClaimsAsync(user);

            model.AllRoles = allRoles;
            model.AllClaims = allClaims.ToList();
            model.UserClaims = userClaims.ToList();
            model.UserRoles = new List<IdentityRole>();

            //loop though the role and role name of the user get
            foreach (var role in allRoles)
            {
                foreach (var userRole in userRoles)
                {
                    //if the user get that role, add to a list
                    if (role.Name.Equals(userRole))
                    {
                        model.UserRoles.Add(role);
                        break;
                    }
                }
            }

            model.AllRolesText = new List<SelectListItem>();
            AddDropList(model, "---Select---");
            AddDropList(model, "Custom");
            //add all roles to the drop down list id not exsits
            foreach (var role in model.AllRoles)
            {
                AddDropList(model, role.Name);
            }

            model.RoleClaims = new List<List<string>>();

            //loop though the role and claims of that role
            //fill up the list with a list of a pair of role name nad claim name
            foreach (var role in model.AllRoles)
            {
                var claimList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimList)
                {
                    List<string> roleClaim = new List<string>
                    {
                        role.Name,
                        claim.Type
                    };
                    model.RoleClaims.Add(roleClaim);
                }
            }
        }

        //Add item to the drop down list if item not exsits
        private void AddDropList(UserViewModel model, string text)
        {
            SelectListItem item = new SelectListItem { Value = text, Text = text };
            SelectListItem itemToAdd = model.AllRolesText.FirstOrDefault(i => i.Equals(item));

            //if item not exsits, add
            if (itemToAdd == null)
            {
                model.AllRolesText.Add(item);
            }
        }

        //Get the user from database which the user want to delete and display details on the page
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            //if user does not exsits, return not found
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        /*
            Add the user created to database if all requried data is given
            Redirect to the page for editing user for the created user after creation
            to let user give permissions to that user
         */
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            //Check if all required data is given
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                //if the user doesn not exsits, create the user
                if (user == null)
                {
                    var identityUser = new IdentityUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };
                    await _userManager.CreateAsync(identityUser, model.Password);
                    return RedirectToAction("EditUser", "AdminPanel", new { id = identityUser.Id });
                }
            }
            return View();
        }

        [HttpPost]
        /*
            Update the user created by user in database if all requried data is given
            Redirect to the page showing details of the updated post after editing
         */
        public async Task<IActionResult> EditUser(UserViewModel model, string save,
            string add, string remove)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            //if the save button is pressed
            if (!string.IsNullOrEmpty(save))
            {
                //Check if all required data is given
                if (ModelState.IsValid)
                {
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;
                    user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    user.PhoneNumber = model.PhoneNumber;

                    await _userManager.UpdateAsync(user);

                    //Update the permissions if Custom is choosen in the drop down list
                    if (model.Role.Equals("Custom"))
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRoles);

                        var claims = await _userManager.GetClaimsAsync(user);
                        await _userManager.RemoveClaimsAsync(user, claims);

                        await AddClaims(user, _userManager, model);
                    }

                    return RedirectToAction("User", "AdminPanel", new { id = user.Id });
                }
            }
            //if the add role to user button is pressed
            if (!string.IsNullOrEmpty(add))
            {
                //if the user does not get that role, add the role to the user
                if (!await _userManager.IsInRoleAsync(user, model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                var role = await _roleManager.FindByNameAsync(model.Role);
                var storedClaims = await _roleManager.GetClaimsAsync(role);
                //add claims of that role to the user
                foreach (var elem in storedClaims)
                {
                    await AddClaim(user, _userManager, elem.Type, elem.Value);
                }

                await EditView(model.Id, model);
                return View(model);
            }
            //if remove role from user button is pressed
            if (!string.IsNullOrEmpty(remove))
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var role = await _roleManager.FindByNameAsync(model.Role);
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                await _userManager.RemoveClaimsAsync(user, roleClaims);
                await _userManager.RemoveFromRoleAsync(user, role.Name);

                var userRoles = await _userManager.GetRolesAsync(user);

                //loop though the roles of the user get and claims in that role
                foreach (var userRole in userRoles)
                {
                    var currentRole = await _roleManager.FindByNameAsync(userRole);
                    var currentRoleClaims = await _roleManager.GetClaimsAsync(currentRole);
                    foreach (var roleClaim in currentRoleClaims)
                    {
                        await AddClaim(user, _userManager, roleClaim.Type, roleClaim.Value);
                    }
                }

                await EditView(model.Id, model);
                return View(model);
            }
            return View(model);
        }

        //add that claim to user if the check box is selected
        private static async Task AddClaims(IdentityUser user,
            UserManager<IdentityUser> userManager, UserViewModel model)
        {
            //if the check box is selected, add claim to user
            if (model.CanCreatePost)
            {
                await AddClaim(user, userManager, "Create Post", "allowed");
            }
            if (model.CanEditPost)
            {
                await AddClaim(user, userManager, "Edit Post", "allowed");
            }
            if (model.CanDeletePost)
            {
                await AddClaim(user, userManager, "Delete Post", "allowed");
            }
            if (model.CanViewPost)
            {
                await AddClaim(user, userManager, "View Post", "allowed");
            }
            if (model.CanViewPostList)
            {
                await AddClaim(user, userManager, "View Post List", "allowed");
            }
            if (model.CanCreateComment)
            {
                await AddClaim(user, userManager, "Create Comment", "allowed");
            }
            if (model.CanEditComment)
            {
                await AddClaim(user, userManager, "Edit Comment", "allowed");
            }
            if (model.CanDeleteComment)
            {
                await AddClaim(user, userManager, "Delete Comment", "allowed");
            }
            if (model.CanViewComment)
            {
                await AddClaim(user, userManager, "View Comment", "allowed");
            }
            if (model.CanLike)
            {
                await AddClaim(user, userManager, "Like", "allowed");
            }
            if (model.CanDislike)
            {
                await AddClaim(user, userManager, "Dislike", "allowed");
            }
            if (model.CanPermissionPanel)
            {
                await AddClaim(user, userManager, "Permission Panel", "allowed");
            }
        }

        //Add a claim to user if user does not get that claim
        private static async Task AddClaim(IdentityUser user,
            UserManager<IdentityUser> userManager, string claim, string value)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var claimToAdd = userClaims.FirstOrDefault(c => c.Type.Equals(claim));

            //if the user does not get the claim to add, add the claim to the user
            if (claimToAdd == null)
            {
                await userManager.AddClaimAsync(user, new Claim(claim, value));
            }
        }

        //Add a claim to role if role does not get that claim
        private static async Task AddClaim(IdentityRole role,
            RoleManager<IdentityRole> roleManager, string claim, string value)
        {
            var storedClaims = await roleManager.GetClaimsAsync(role);
            var claimToAdd = storedClaims.FirstOrDefault(c => c.Type.Equals(claim));

            //if the role does not get the claim to add, add the claim to the user
            if (claimToAdd == null)
            {
                await roleManager.AddClaimAsync(role, new Claim(claim, value));
            }
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        /*
            Find the user which the user want to delete in database
            and remove it and its roles and claims from the database
            Redirect to the page showing list of users after deletion
         */
        public async Task<IActionResult> DeleteUserAndRelated(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveClaimsAsync(user, claims);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("UserList");
        }

        //Get all roles from database and turn it to a list. Then display all roles in a page
        public async Task<IActionResult> RoleList()
        {
            List<IdentityRole> roleList = await _repo.GetRoleList();
            return View(roleList);
        }

        //Get the requested role from database and display the details of the role
        public async Task<IActionResult> Role(string id)
        {
            return await GetRoleView(id);
        }

        //Get the role from database which the user want to delete and display details on the page
        public async Task<IActionResult> DeleteRole(string id)
        {
            return await GetRoleView(id);
        }

        //Get all the data needed for displaying the page
        private async Task<IActionResult> GetRoleView(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var allRoles = _roleManager.Roles.ToList();

            //if the requested role does not exsits, return Not found
            if (role == null)
            {
                return NotFound();
            }

            RoleViewModel model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                AllClaims = allClaims.ToList(),
                AllRoles = allRoles,

                RoleClaims = new List<List<string>>()
            };

            //loop though the role and claims of that role
            //fill up the list with a list of a pair of role name nad claim name
            foreach (var role1 in model.AllRoles)
            {
                var claimList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimList)
                {
                    List<string> roleClaim = new List<string>
                    {
                        role1.Name,
                        claim.Type
                    };
                    model.RoleClaims.Add(roleClaim);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        //delete the role and its claims from database 
        //and delete this role and its unique claims from users
        public async Task<IActionResult> DeleteRoleAndRelated(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var users = await _userManager.Users.ToListAsync();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            //remove all claims of that role
            foreach (var claim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            await _roleManager.DeleteAsync(role);

            //remove the role and its claims form all users
            foreach (var user in users)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                await _userManager.RemoveClaimsAsync(user, roleClaims);
                await _userManager.RemoveFromRoleAsync(user, role.Name);

                var userRoles = await _userManager.GetRolesAsync(user);

                //Add back the claims removed for other role also get
                foreach (var userRole in userRoles)
                {
                    var currentRole = await _roleManager.FindByNameAsync(userRole);
                    var currentRoleClaims = await _roleManager.GetClaimsAsync(currentRole);
                    foreach (var roleClaim in currentRoleClaims)
                    {
                        await AddClaim(user, _userManager, roleClaim.Type, roleClaim.Value);
                    }
                }
            }

            return RedirectToAction("RoleList");
        }

        //Get the role from database which the user want to edit and display details on input area
        public async Task<IActionResult> EditRole(string id)
        {
            return await GetRoleView(id);
        }

        [HttpPost]
        /*
           Update the role name and its claims in database if all requried data is given
           Redirect to the page showing details of the updated role after editing
        */
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            //Check if all required data is given
            if (ModelState.IsValid)
            {
                role.Name = model.Name;

                await _roleManager.UpdateAsync(role);

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                //Remove all claims of the editing role
                foreach (var claim in roleClaims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                await AddClaims(role, _roleManager, model);
            }

            return RedirectToAction("Role", "AdminPanel", new { id = role.Id });
        }

        //add that claim to role if the check box is selected
        private static async Task AddClaims(IdentityRole role,
            RoleManager<IdentityRole> roleManager, RoleViewModel model)
        {
            //if the check box is selected, add claim to role
            if (model.CanCreatePost)
            {
                await AddClaim(role, roleManager, "Create Post", "allowed");
            }
            if (model.CanEditPost)
            {
                await AddClaim(role, roleManager, "Edit Post", "allowed");
            }
            if (model.CanDeletePost)
            {
                await AddClaim(role, roleManager, "Delete Post", "allowed");
            }
            if (model.CanViewPost)
            {
                await AddClaim(role, roleManager, "View Post", "allowed");
            }
            if (model.CanViewPostList)
            {
                await AddClaim(role, roleManager, "View Post List", "allowed");
            }
            if (model.CanCreateComment)
            {
                await AddClaim(role, roleManager, "Create Comment", "allowed");
            }
            if (model.CanEditComment)
            {
                await AddClaim(role, roleManager, "Edit Comment", "allowed");
            }
            if (model.CanDeleteComment)
            {
                await AddClaim(role, roleManager, "Delete Comment", "allowed");
            }
            if (model.CanViewComment)
            {
                await AddClaim(role, roleManager, "View Comment", "allowed");
            }
            if (model.CanLike)
            {
                await AddClaim(role, roleManager, "Like", "allowed");
            }
            if (model.CanDislike)
            {
                await AddClaim(role, roleManager, "Dislike", "allowed");
            }
            if (model.CanPermissionPanel)
            {
                await AddClaim(role, roleManager, "Permission Panel", "allowed");
            }
        }

        //Display the page of creating role
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        /*
            Add the role and its claims created by user to database if all requried data is given
            Redirect to the page showing details of the created role after creation
         */
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            //Check if all required data is given
            if (ModelState.IsValid)
            {
                //If the role does not exsits, add the role and its selected claims
                if (!await _roleManager.RoleExistsAsync(model.Name))
                {
                    IdentityRole role = new IdentityRole(model.Name);
                    await _roleManager.CreateAsync(role);
                    await AddClaims(role, _roleManager, model);

                    return RedirectToAction("Role", "AdminPanel", new { id = role.Id });
                }
            }

            return View();
        }
    }
}
