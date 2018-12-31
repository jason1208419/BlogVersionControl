using CSC348Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Data.Repository
{
    public interface IRepo
    {
        Task<Post> GetPost(int? id);
        Task<List<Post>> GetPostList();
        Task AddPost(Post post);
        Task SaveChangesAsync();
        void UpdatePost(Post post);
        void RemovePost(Post post);
    }
}
