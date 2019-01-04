using CS348Blog.Data;
using CSC348Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Data.Repository
{
    public class Repo : IRepo
    {
        private ApplicationDbContext _db;

        public Repo (ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Post> GetPost (int? id)
        {
            return await _db.Posts.Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostID == id);
        }

        public async Task<List<Post>> GetPostList()
        {
            return await _db.Posts.ToListAsync();
        }

        public async Task AddPost(Post post)
        {
            await _db.Posts.AddAsync(post);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdatePost(Post post)
        {
            _db.Posts.Update(post);
        }

        public void RemovePost(Post post)
        {
            _db.Posts.Remove(post);
        }

        public async Task<List<IdentityUser>> GetUserList()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<List<IdentityRole>> GetRoleList()
        {
            return await _db.Roles.ToListAsync();
        }
    }
}
