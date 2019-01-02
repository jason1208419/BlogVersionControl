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
using System.Security.Claims;

/*
    This controller due with Get request and post request about posts
    Only admin can do these get and post requests
 */
namespace CSC348Blog.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PostController : Controller
    {
        private IRepo _repo;

        //Initialize the database when construting the controller
        public PostController(IRepo repo)
        {
            _repo = repo;
            //var comment = new Comment();
        }

        //Get the requested post from database and display the details of the post
        [Authorize("View Post")]
        public async Task<IActionResult> Post(int? id)
        {
            return await GetPost(id);
        }

        //Display the page of creating post
        [Authorize("Create Post")]
        public IActionResult CreatePost()
        {
            return View();
        }

        //Get all posts from database and turn it to a list. Then display all posts in a page
        [Authorize("View Post List")]
        public async Task<IActionResult> PostList()
        {
            List<Post> postsList = await _repo.GetPostList();
            return View(postsList);
        }

        //Get the post from database which the user want to edit and display details on input area
        //[Authorize("Edit Post")]
        public async Task<IActionResult> EditPost(int? id)
        {
            return await Authorize(id, "edit");
        }

        //Get the post from database which the user want to delete and display details on the page
        //[Authorize("Delete Post")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            return await Authorize(id, "delete");
        }

        /*
            Use the post created by user and add to database if all requried data is given
            Redirect to the page showing details of the created post after creation
         */
        [HttpPost]
        [Authorize("Create Post")]
        public async Task<IActionResult> CreatePost(Post post)
        {
            post.Creator = User.Identity.Name;
            post.CreationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

            //Check if all required data is given
            if (ModelState.IsValid)
            {
                await _repo.AddPost(post);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Post", "Post", new { id = post.PostID });
            }

            return View(post);
        }

        /*
            Use the post created by user and update that post in database if all requried data is given
            Redirect to the page showing details of the created post after editing
         */
        [HttpPost]
        //[Authorize("Edit Post")]
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

                _repo.UpdatePost(post);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Post", "Post", new { id = post.PostID });
            }
            return View(post);
        }

        /*
            Find the post the user want to delete in database and remove it from the database
            Redirect to the page showing list of posts after deletion
         */
        [HttpPost]
        //[Authorize("Delete Post")]
        public async Task<IActionResult> DeletePost(int id)
        {
            Post post = await _repo.GetPost(id);

            _repo.RemovePost(post);
            await _repo.SaveChangesAsync();

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

            Post post = await _repo.GetPost(id);

            //Check if the post is found in the database base on the given post ID
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        private async Task<bool> IsPostOwner(int? id, string action)
        {
            if (id == null)
            {
                return false;
            }

            var post = await _repo.GetPost(id);

            if (post == null)
            {
                return false;
            }

            if (action.Equals("edit", StringComparison.InvariantCultureIgnoreCase))
            {
                if (post.Creator.Equals(User.Identity.Name) || User.HasClaim("Edit Post", "allowed"))
                {
                    return true;
                }
            } else if (action.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                if (post.Creator.Equals(User.Identity.Name) || User.HasClaim("Delete Post", "allowed"))
                {
                    return true;
                }
            }

            
            return false;
        }

        private async Task<IActionResult> Authorize(int? id, string action)
        {
            if (!await IsPostOwner(id, action))
            {
                return Redirect("/Identity/Account/AccessDenied");
            }
            return await GetPost(id);
        }

        [HttpPost]
        [Authorize("Create Comment")]
        public async Task<IActionResult> Comment(CommentViewModel fuck)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = fuck.PostID });
            }

            var post = await _repo.GetPost(fuck.PostID);
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
            _repo.UpdatePost(post);
            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = fuck.PostID });
        }
    }
}