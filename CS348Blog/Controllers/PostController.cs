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
    This controller deal with Get request and post request about posts
    Only admin can do get and post requests of post
    All user can do get and post requests of comment
 */
namespace CSC348Blog.Models
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
        }

        //Get the requested post from database and display the details of the post
        [AllowAnonymous]
        public async Task<IActionResult> Post(int? id)
        {
            //Check if post ID is given for finding the post
            if (id == null)
            {
                return NotFound();
            }

            var post = await _repo.GetPost(id);

            //Check if the post is found in the database base on the given post ID
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        //Display the page of creating post
        [Authorize("Create Post")]
        public IActionResult CreatePost()
        {
            return View();
        }

        //Get all posts from database and turn it to a list. Then display all posts in a page
        [AllowAnonymous]
        public async Task<IActionResult> PostList()
        {
            List<Post> postsList = await _repo.GetPostList();

            return View(postsList);
        }

        //Get the post from database which the user want to edit and display details on input area
        public async Task<IActionResult> EditPost(int? id)
        {
            return await Authorize(id, "edit");
        }

        //Get the post from database which the user want to delete and display details on the page
        public async Task<IActionResult> DeletePost(int? id)
        {
            return await Authorize(id, "delete");
        }

        /*
            Add the post created by user to database if all requried data is given
            Redirect to the page showing details of the created post after creation
         */
        [HttpPost]
        [Authorize("Create Post")]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            Post newPost = new Post
            {
                Creator = User.Identity.Name,
                CreationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                Title = model.Title,
                Content = model.Content
            };

            //Check if all required data is given
            if (ModelState.IsValid)
            {
                await _repo.AddPost(newPost);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Post", "Post", new { id = newPost.PostID });
            }

            return View(model);
        }

        /*
            Update the post created by user in database if all requried data is given
            Redirect to the page showing details of the updated post after editing
         */
        [HttpPost]
        public async Task<IActionResult> EditPost(int id, PostViewModel model)
        {
            var post = await _repo.GetPost(id);
            //Check if the user edit the post he meant to be editing
            if (id != post.PostID)
            {
                return NotFound();
            }

            post.Editor = User.Identity.Name;
            post.EditDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            post.Title = model.Title;
            post.Content = model.Content;

            //Check if all required data is given
            if (ModelState.IsValid)
            {
                _repo.UpdatePost(post);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Post", "Post", new { id = post.PostID });
            }
            return View(model);
        }

        /*
            Find the post the user want to delete in database and remove it from the database
            Redirect to the page showing list of posts after deletion
         */
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            Post post = await _repo.GetPost(id);

            _repo.RemovePost(post);
            await _repo.SaveChangesAsync();

            return RedirectToAction("PostList");
        }

        //Check if the user is the owner of the post or get right to edit or delete all posts
        private bool IsPostOwner(int? id, string action, Post post)
        {
            //if user want to edit
            if (action.Equals("edit", StringComparison.InvariantCultureIgnoreCase))
            {
                //if the user is the owner of the post or get right to edit, return true
                if (post.Creator.Equals(User.Identity.Name) || User.HasClaim("Edit Post", "allowed"))
                {
                    return true;
                }
            }
            //if user want to delete
            else if (action.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                //if the user is the owner of the post or get right to delete, return true
                if (post.Creator.Equals(User.Identity.Name) || User.HasClaim("Delete Post", "allowed"))
                {
                    return true;
                }
            }

            return false;
        }

        //Return a view of PostViewModel if user is owner of post or get claim of the action
        private async Task<IActionResult> Authorize(int? id, string action)
        {
            //Check if post ID is given for finding the post
            if (id == null)
            {
                return NotFound();
            }

            var post = await _repo.GetPost(id);

            //Check if the post is found in the database base on the given post ID
            if (post == null)
            {
                return NotFound();
            }

            //if the user does not have right, retun view of Access Denied
            if (!IsPostOwner(id, action, post))
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            PostViewModel model = new PostViewModel
            {
                Title = post.Title,
                Content = post.Content
            };

            return View(model);
        }

        [HttpPost]
        [Authorize("Create Comment")]
        //To create a comment
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            //To check if all required data filled
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostID });
            }

            var post = await _repo.GetPost(model.PostID);
            post.Comments = post.Comments ?? new List<Comment>();

            //if the parent of the comment is the post
            if (model.ParentCommentID == 0)
            {
                post.Comments.Add(new Comment
                {
                    Content = model.Content,
                    Creator = User.Identity.Name,
                    CreationTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now)
                });
            }
            //if the parent of the comment is a comment
            else
            {
                post.Comments.Add(new Comment
                {
                    ParentCommentID = model.ParentCommentID,
                    Content = "Reply To " + model.ReplyTo + " ------ " + model.Content,
                    Creator = User.Identity.Name,
                    CreationTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now)
                });
            }
            _repo.UpdatePost(post);
            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = model.PostID });
        }
    }
}