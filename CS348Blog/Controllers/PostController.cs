using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSC348Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CS348Blog.Data;
using CSC348Blog.Data.Repository;

/*
    This controller due with Get request and post request about posts
    Only admin can do these get and post requests
 */
namespace CSC348Blog.Controllers
{
    [Authorize/*(Policy = "AdminOnly")*/]
    public class PostController : Controller
    {
        private IRepo repo;

        //Initialize the database when construting the controller
        public PostController(IRepo _repo)
        {
            repo = _repo;
            //var comment = new Comment();
        }

        //Get the requested post from database and display the details of the post
        public async Task<IActionResult> Post(int? id)
        {
            return await GetPost(id);
        }

        //Display the page of creating post
        public IActionResult CreatePost()
        {
            return View();
        }

        //Get all posts from database and turn it to a list. Then display all posts in a page
        public async Task<IActionResult> PostList()
        {
            List<Post> postsList = await repo.GetPostList();
            return View(postsList);
        }

        //Get the post from database which the user want to edit and display details on input area
        public async Task<IActionResult> EditPost(int? id)
        {
            return await GetPost(id);
        }

        //Get the post from database which the user want to delete and display details on the page
        public async Task<IActionResult> DeletePost(int? id)
        {
            return await GetPost(id);
        }

        /*
            Use the post created by user and add to database if all requried data is given
            Redirect to the page showing details of the created post after creation
         */
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            post.Creator = User.Identity.Name;
            post.CreationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

            //Check if all required data is given
            if (ModelState.IsValid)
            {
                await repo.AddPost(post);
                await repo.SaveChangesAsync();

                return RedirectToAction("Post", "Post", new { id = post.PostID });
            }

            return View(post);
        }

        /*
            Use the post created by user and update that post in database if all requried data is given
            Redirect to the page showing details of the created post after editing
         */
        [HttpPost]
        public async Task<IActionResult> EditPost(int id, Post post)
        {
            //Check if the user edit the post he meant to be editing
            if (id != post.PostID)
            {
                return NotFound();
            }

            post.Editor = User.Identity.Name;
            post.EditDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

            //Check if all required data is given
            if (ModelState.IsValid)
            {

                repo.UpdatePost(post);
                await repo.SaveChangesAsync();

                return RedirectToAction("Post", "Post", new { id = post.PostID });
            }
            return View(post);
        }

        /*
            Find the post the user want to delete in database and remove it from the database
            Redirect to the page showing list of posts after deletion
         */
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            Post post = await repo.GetPost(id);

            repo.RemovePost(post);
            await repo.SaveChangesAsync();

            return RedirectToAction("PostList");
        }

        //Get the post requested from the database and pass the post to View
        private async Task<IActionResult> GetPost(int? id)
        {
            //Check if post ID is given for finding the post
            if (id == null)
            {
                return NotFound();
            }

            Post post = await repo.GetPost(id);

            //Check if the post is found in the database base on the given post ID
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel fuck)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = fuck.PostID });
            }

            var post = await repo.GetPost(fuck.PostID);
            post.Comments = post.Comments ?? new List<Comment>();
            if (fuck.MainCommentID == 0)
            {
                post.Comments.Add(new Comment
                {
                    Content = fuck.Content,
                    Creator = User.Identity.Name,
                    CreationTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now)
                });
            }
            else
            {
                post.Comments.Add(new Comment
                {
                    ParentCommentID = fuck.MainCommentID,
                    Content = "@" + fuck.ReplyTo + "    " + fuck.Content,
                    Creator = User.Identity.Name,
                    CreationTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now)
                });
            }
            repo.UpdatePost(post);
            await repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = fuck.PostID });
        }
    }
}