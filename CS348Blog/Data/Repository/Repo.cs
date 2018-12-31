using CS348Blog.Data;
using CSC348Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Data.Repository
{
    public class Repo : IRepo
    {
        private ApplicationDbContext db;

        public Repo (ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<Post> GetPost (int? id)
        {
            return await db.Posts.Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostID == id);
        }

        public async Task<List<Post>> GetPostList()
        {
            return await db.Posts.ToListAsync();
        }

        public async Task AddPost(Post post)
        {
            await db.Posts.AddAsync(post);
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

        public void UpdatePost(Post post)
        {
            db.Posts.Update(post);
        }

        public void RemovePost(Post post)
        {
            db.Posts.Remove(post);
        }
    }
}
