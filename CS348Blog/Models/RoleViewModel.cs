using Microsoft.AspNetCore.Identity;
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
    public class RoleViewModel
    {
        [ReadOnly(true)]
        public string Id { get; set; }

        [Required, MinLength(2), MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public bool CanCreatePost { get; set; }
        [Required]
        public bool CanEditPost { get; set; }
        [Required]
        public bool CanDeletePost { get; set; }
        [Required]
        public bool CanViewPost { get; set; }
        [Required]
        public bool CanViewPostList { get; set; }
        [Required]
        public bool CanCreateComment { get; set; }
        [Required]
        public bool CanEditComment { get; set; }
        [Required]
        public bool CanDeleteComment { get; set; }
        [Required]
        public bool CanViewComment { get; set; }
        [Required]
        public bool CanLike { get; set; }
        [Required]
        public bool CanDislike { get; set; }
        [Required]
        public bool CanPermissionPanel { get; set; }

        [ReadOnly(true)]
        public List<IdentityRole> AllRoles { get; set; }
        [ReadOnly(true)]
        public List<Claim> AllClaims { get; set; }
        [ReadOnly(true)]
        public List<List<string>> RoleClaims { get; set; }
    }
}
