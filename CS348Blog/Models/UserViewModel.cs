using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    //To get and save data from/to storage for AdminPanel controller.
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public string Role { get; set; }
        public bool CanCreatePost { get; set; }
        public bool CanEditPost { get; set; }
        public bool CanDeletePost { get; set; }
        public bool CanViewPost { get; set; }
        public bool CanViewPostList { get; set; }
        public bool CanCreateComment { get; set; }
        public bool CanEditComment { get; set; }
        public bool CanDeleteComment { get; set; }
        public bool CanViewComment { get; set; }
        public bool CanLike { get; set; }
        public bool CanDislike { get; set; }
        public bool CanPermissionPanel { get; set; }
        public List<SelectListItem> AllRolesText { get; set; }

        public List<IdentityRole> AllRoles { get; set; }
        public List<IdentityRole> UserRoles { get; set; }
        public List<Claim> AllClaims { get; set; }
        public List<Claim> UserClaims { get; set; }
        public List<List<string>> RoleClaims { get; set; }
    }
}
