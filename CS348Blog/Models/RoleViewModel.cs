using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

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

        public List<IdentityRole> AllRoles { get; set; }
        public List<Claim> AllClaims { get; set; }
        public List<List<string>> RoleClaims { get; set; }
    }
}
