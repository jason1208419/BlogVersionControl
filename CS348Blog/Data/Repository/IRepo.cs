using CSC348Blog.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Data.Repository
{
    //An interface to let controller get data from and save data to database
    public interface IRepo
    {
        Task<Post> GetPost(int? id);
        Task<List<Post>> GetPostList();
        Task AddPost(Post post);
        Task SaveChangesAsync();
        void UpdatePost(Post post);
        void RemovePost(Post post);
        Task<List<IdentityUser>> GetUserList();
        Task<List<IdentityRole>> GetRoleList();
    }
}
