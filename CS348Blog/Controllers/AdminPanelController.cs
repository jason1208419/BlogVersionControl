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
    public class AdminPanelController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IRepo _repo;

        public AdminPanelController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRepo repo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _repo = repo;
        }

        public async Task<IActionResult> UserList()
        {
            var userList = await _repo.GetUserList();
            return View(userList);
        }

        public async Task<IActionResult> User(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            UserViewModel model = new UserViewModel();
            model.Id = user.Id;
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.EmailConfirmed = user.EmailConfirmed;
            model.PhoneNumber = user.PhoneNumber;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            model.TwoFactorEnabled = user.TwoFactorEnabled;
            model.LockoutEnabled = user.LockoutEnabled;
            model.AccessFailedCount = user.AccessFailedCount;

            model.AllClaims = allClaims.ToList();
            model.UserClaims = userClaims.ToList();
            model.AllRoles = allRoles;
            model.UserRoles = new List<IdentityRole>();
            foreach (var role in allRoles)
            {
                foreach (var userRole in userRoles)
                {
                    if (role.Name.Equals(userRole))
                    {
                        model.UserRoles.Add(role);
                        break;
                    }
                }
            }

            return View(model);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            UserViewModel model = new UserViewModel();
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.EmailConfirmed = user.EmailConfirmed;
            model.PhoneNumber = user.PhoneNumber;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

            await EditView(id, model);

            return View(model);
        }

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
            foreach (var role in allRoles)
            {
                foreach (var userRole in userRoles)
                {
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
            foreach (var role in model.AllRoles)
            {
                AddDropList(model, role.Name);
            }

            model.RoleClaims = new List<List<string>>();
            foreach (var role in model.AllRoles)
            {
                var claimList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimList)
                {
                    List<string> roleClaim = new List<string>();
                    roleClaim.Add(role.Name);
                    roleClaim.Add(claim.Type);
                    model.RoleClaims.Add(roleClaim);
                }
            }
        }

        private void AddDropList(UserViewModel model, string text)
        {
            SelectListItem item = new SelectListItem { Value = text, Text = text };

            if (!IsExsits(model, item))
            {
                model.AllRolesText.Add(new SelectListItem { Value = text, Text = text });
            }
        }

        private bool IsExsits(UserViewModel model, SelectListItem item)
        {
            foreach (var elem in model.AllRolesText)
            {
                if (elem.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    var identityUser = new IdentityUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };
                    await _userManager.CreateAsync(identityUser, model.Password);
                    return RedirectToAction("EditUser", "AdminPanel", new { id = identityUser.Id });
                    //return RedirectToAction("UserList", "AdminPanel");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model, string save, string add, string remove)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;
                    user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    user.PhoneNumber = model.PhoneNumber;

                    await _userManager.UpdateAsync(user);

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
            if (!string.IsNullOrEmpty(add))
            {
                if (!await _userManager.IsInRoleAsync(user, model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                var role = await _roleManager.FindByNameAsync(model.Role);
                var storedClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var elem in storedClaims)
                {
                    await AddClaim(user, _userManager, elem.Type, elem.Value);
                }

                await EditView(model.Id, model);
                return View(model);
            }
            if (!string.IsNullOrEmpty(remove))
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var role = await _roleManager.FindByNameAsync(model.Role);
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                await _userManager.RemoveClaimsAsync(user, roleClaims);
                await _userManager.RemoveFromRoleAsync(user, role.Name);

                var userRoles = await _userManager.GetRolesAsync(user);
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

        private static async Task AddClaims(IdentityUser user, UserManager<IdentityUser> userManager, UserViewModel model)
        {
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

        private static async Task AddClaim(IdentityUser user, UserManager<IdentityUser> userManager, string claim, string value)
        {
            var storedClaims = await userManager.GetClaimsAsync(user);
            var claimToCheck = storedClaims.FirstOrDefault(c => c.Type == claim);

            if (claimToCheck == null)
            {
                await userManager.AddClaimAsync(user, new Claim(claim, value));
            }
        }

        private static async Task AddClaim(IdentityRole role, RoleManager<IdentityRole> roleManager, string claim, string value)
        {
            var storedClaims = await roleManager.GetClaimsAsync(role);
            var claimToCheck = storedClaims.FirstOrDefault(c => c.Type == claim);

            if (claimToCheck == null)
            {
                await roleManager.AddClaimAsync(role, new Claim(claim, value));
            }
        }

        [HttpPost]
        [ActionName("DeleteUser")]
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

        public async Task<IActionResult> RoleList()
        {
            List<IdentityRole> roleList = await _repo.GetRoleList();
            return View(roleList);
        }

        public async Task<IActionResult> Role(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var allRoles = _roleManager.Roles.ToList();

            if (role == null)
            {
                return NotFound();
            }

            RoleViewModel model = new RoleViewModel();
            model.Id = role.Id;
            model.Name = role.Name;
            model.AllClaims = allClaims.ToList();
            model.AllRoles = allRoles;

            model.RoleClaims = new List<List<string>>();
            foreach (var role1 in model.AllRoles)
            {
                var claimList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimList)
                {
                    List<string> roleClaim = new List<string>();
                    roleClaim.Add(role1.Name);
                    roleClaim.Add(claim.Type);
                    model.RoleClaims.Add(roleClaim);
                }
            }


            return View(model);
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var allRoles = _roleManager.Roles.ToList();

            if (role == null)
            {
                return NotFound();
            }

            RoleViewModel model = new RoleViewModel();
            model.Id = role.Id;
            model.Name = role.Name;
            model.AllClaims = allClaims.ToList();
            model.AllRoles = allRoles;

            model.RoleClaims = new List<List<string>>();
            foreach (var role1 in model.AllRoles)
            {
                var claimList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimList)
                {
                    List<string> roleClaim = new List<string>();
                    roleClaim.Add(role1.Name);
                    roleClaim.Add(claim.Type);
                    model.RoleClaims.Add(roleClaim);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleAndRelated(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var users = await _userManager.Users.ToListAsync();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            await _roleManager.DeleteAsync(role);

            foreach (var user in users)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                await _userManager.RemoveClaimsAsync(user, roleClaims);
                await _userManager.RemoveFromRoleAsync(user, role.Name);

                var userRoles = await _userManager.GetRolesAsync(user);
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

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var admin = await _userManager.FindByNameAsync("Member1@email.com");
            var allClaims = await _userManager.GetClaimsAsync(admin);
            var allRoles = _roleManager.Roles.ToList();

            if (role == null)
            {
                return NotFound();
            }

            RoleViewModel model = new RoleViewModel();
            model.Id = role.Id;
            model.Name = role.Name;
            model.AllClaims = allClaims.ToList();
            model.AllRoles = allRoles;

            model.RoleClaims = new List<List<string>>();
            foreach (var role1 in model.AllRoles)
            {
                var claimList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimList)
                {
                    List<string> roleClaim = new List<string>();
                    roleClaim.Add(role1.Name);
                    roleClaim.Add(claim.Type);
                    model.RoleClaims.Add(roleClaim);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (ModelState.IsValid)
            {
                role.Name = model.Name;

                await _roleManager.UpdateAsync(role);

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in roleClaims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                await AddClaims(role, _roleManager, model);
            }

            return RedirectToAction("Role", "AdminPanel", new { id = role.Id });
        }

        private static async Task AddClaims(IdentityRole role, RoleManager<IdentityRole> roleManager, RoleViewModel model)
        {
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

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
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
