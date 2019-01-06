using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    //To get and save data from/to storage for AdminPanel controller.
    public class UserViewModel
    {
        [ReadOnly(true)]
        public string Id { get; set; }

        [Required, ReadOnly(true)]
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

        [ReadOnly(true)]
        public bool TwoFactorEnabled { get; set; }

        [ReadOnly(true)]
        public bool LockoutEnabled { get; set; }

        [ReadOnly(true)]
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
        [ReadOnly(true)]
        public List<SelectListItem> AllRolesText { get; set; }

        [ReadOnly(true)]
        public List<IdentityRole> AllRoles { get; set; }
        [ReadOnly(true)]
        public List<IdentityRole> UserRoles { get; set; }
        [ReadOnly(true)]
        public List<Claim> AllClaims { get; set; }
        [ReadOnly(true)]
        public List<Claim> UserClaims { get; set; }
        [ReadOnly(true)]
        public List<List<string>> RoleClaims { get; set; }
    }
}
