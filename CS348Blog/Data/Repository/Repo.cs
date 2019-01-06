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
    //To get and save data from/to database
    public class Repo : IRepo
    {
        private ApplicationDbContext _db;

        //initialise the repository
        public Repo (ApplicationDbContext db)
        {
            _db = db;
        }

        //Get a post with comments
        public async Task<Post> GetPost (int? id)
        {
            return await _db.Posts.Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostID == id);
        }

        //Get a list of all posts
        public async Task<List<Post>> GetPostList()
        {
            return await _db.Posts.ToListAsync();
        }

        //Add a post
        public async Task AddPost(Post post)
        {
            await _db.Posts.AddAsync(post);
        }

        //Save changes to database
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        //Update a post
        public void UpdatePost(Post post)
        {
            _db.Posts.Update(post);
        }

        //Remove a post
        public void RemovePost(Post post)
        {
            _db.Posts.Remove(post);
        }

        //Get a list of users
        public async Task<List<IdentityUser>> GetUserList()
        {
            return await _db.Users.ToListAsync();
        }

        //Get a list of roles
        public async Task<List<IdentityRole>> GetRoleList()
        {
            return await _db.Roles.ToListAsync();
        }
    }
}
