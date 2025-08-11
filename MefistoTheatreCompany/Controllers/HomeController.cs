// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MefistoTheatreCompany.Models;
using MefistoTheatreCompany.Models.ViewModels;
using System.Security.Permissions;
using System.Web.Services.Description;
using System.Web.UI;
using System.Diagnostics;
using System.Web.WebPages;

namespace MefistoTheatreCompany.Controllers
{
    public class HomeController : Controller
    {

        //create an instance of the database context
        private MefistoTheatreCompanyDbContext context = new MefistoTheatreCompanyDbContext();

        public ActionResult SplashScreen()
        {
            return View();
        }

        
        public ActionResult Index()
        {
            ////retrieve usersettings cookie from the request
            //HttpCookie mefistoCookie = Request.Cookies["UserSettings"];

            ////check if the cookie is null
            //if(mefistoCookie == null)
            //{
            //    mefistoCookie = new HttpCookie("UserSettings");
            //    mefistoCookie.Value = "null";
            //    mefistoCookie.Expires = DateTime.Now.AddDays(1);
            //    Response.Cookies.Add(mefistoCookie);
            //}
          

            ////retrieve value of the cookie from the request
           
            //string userSettings = mefistoCookie.Value;

            


            //get all posts, include the category for each post, include the user who created the post
            //and order the posts from  the most current to old posts
            var posts = context.BlogPosts
                .Include(p => p.Category)
                .Include(p => p.User).Include(p => p.Comments.Select(q=>q.User)).Where(p=>p.Approved==true)
                .OrderByDescending(p => p.DatePosted);

             //send the list of categories over the index page
            //so we can display them
            ViewBag.Categories = context.Categories.ToList();
            //send the posts collection to the view named index
            return View(posts.ToList());
            
        }


        [HttpPost]
        //this action will process the search form on the index page
        //the name of the sting parameter SearchString must be the same
        //with the name of the textbox on the view
        public ViewResult Index(string SearchString)
        {
            //string userSettings = string.Empty;

            ////retrieve cookie from the request
            //HttpCookie retrievedCookie = Request.Cookies["UserSettings"];
           
            ////check is the cookie is exists
            //if(retrievedCookie != null)
            //{
            //    //retrieve the cookie value
            //    userSettings = Request.Cookies["UserSettings"].Value;
                
            //}
           
           

            var blogPosts = context.BlogPosts
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Comments.Select(q => q.User)).Where(p => p.Approved == true)
                .OrderByDescending(p => p.DatePosted);

            ViewBag.Categories = context.Categories.ToList();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                // If a search string is provided, filter the posts by category
                blogPosts = blogPosts
                    .Include(p => p.Category)
                    .Include(p => p.User)
                    .Include(p => p.Comments.Select(q => q.User))
                    .Where(p => p.Category.CategoryName.Equals(SearchString.Trim()))
                    .Where(p => p.Approved == true)
                    .OrderByDescending(p => p.DatePosted);
            }

            
            return View(blogPosts.ToList());
        }



        public ActionResult SetUserNameCookie(string userName)
        {


            if (userName.IsEmpty())
            {
                return RedirectToAction("Login", "Account");
            }
            else { 
            //RETRIEVE cookie from user settings
            HttpCookie mefistoCookie = Request.Cookies["UserSettings"];

            //check if the cookie is null
            if (mefistoCookie == null)
            {
                mefistoCookie = new HttpCookie("UserSettings");
                mefistoCookie.Expires = DateTime.Now.AddDays(1);
            }

            //SET USERNAME
            mefistoCookie["Username"] = userName;

            //update cookie response
            Response.Cookies.Add(mefistoCookie);
            ViewBag.UserSettings = userName;

            return RedirectToAction("Index", "Home");
        }
        }

        //cookie policy
        public ActionResult CookiePolicy()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(int id)
        {
            //search the posts table in the database
            //find post by id
            //return post
            BlogPost blogPost = context.BlogPosts.Include(p => p.Comments.Select(q => q.User)).FirstOrDefault(p=>p.BlogPostId==id);

            //using the foreign key UserId from the post instance
            //find the suer who created the post
            var user = context.Users.Find(blogPost.UserId) as Staff;

            //using the foreign key category Id from these posts
            //find the category that the post belongs to
            var category =context.Categories.Find(blogPost.CategoryId );

            

            //assign the user to the User navigational property in Post
            blogPost.User=user;


            //send the post model to the Details View
            return View(blogPost);
        }


        [HttpGet]
        //only these roles can access the Add comment feature
        [Authorize(Roles ="Admin, Moderator, Employee, Member")]
        public ActionResult AddComment(int id)
        {
            //get the blog post by id
            BlogPost post=context.BlogPosts.Find(id);

            
            CreateCommentViewModel comment=new CreateCommentViewModel();
            if (post!=null)
            {
                comment.BlogPostId = post.BlogPostId;
            }
           //display the view for user to add comment

            return View(comment);

        }
        [HttpPost]
       
        public ActionResult AddComment(CreateCommentViewModel model)
        {

            if (ModelState.IsValid)
            {
                var blogPost = context.BlogPosts.Find(model.BlogPostId);

                if (blogPost!=null)
                {
                    //gets the id of the user who is adding the comment
                    var userId = User.Identity.GetUserId();

                    var comment = new Comment
                    {
                        //creates a new comment
                        Content = model.CommentContent,
                        DatePosted = DateTime.Now,
                        UserId = userId,
                        BlogId = blogPost.BlogPostId
                        
                    };
                    //saves the comment to the database
                    context.Comments.Add(comment);
                    context.SaveChanges();

                    return RedirectToAction("Index", new { id = blogPost.BlogPostId });

                }
            

            }
            return View(model);

        }

        [HttpGet]

        
        [Authorize(Roles = "Admin, Moderator, Employee, Member")]
        public ActionResult EditComment(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find post by Id in the posts table
            Comment comment = context.Comments.Find(id);


            if (comment == null)
            {
                return HttpNotFound("comment is null in http get");
            }

            //displays the comment info

            EditCommentViewModel model = new EditCommentViewModel
            {
                CommentId = comment.CommentId,
                CommentContent = comment.Content,
                DatePosted = comment.DatePosted,
            };


            return View(model);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult EditComment(EditCommentViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (ModelState.IsValid)
            {

                //finds the comment
                Comment comment = context.Comments.Find(model.CommentId);
                if (comment == null)
                {
                    return HttpNotFound("comment is null in Httppost");
                }

                //gets the new content
                comment.Content = model.CommentContent;

                //updates the date edited time to the current time
                comment.DateEdited = DateTime.Now;

                comment.UserId = User.Identity.GetUserId();


                //save changes to the database
                context.Entry(comment).State = EntityState.Modified;

                context.SaveChanges();

                return RedirectToAction("Index");
            }

           
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = context.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound("comment is null in http get");
            }

            return View(comment);

        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            //gets the comment that is selected
            //and deletes it from the database
            Comment comment = context.Comments.Find(id);

            context.Comments.Remove(comment);

            context.SaveChanges();

            return RedirectToAction("Index");


        }
    }
}