// Tiani Perera - Mefisto Theatre Company 03/02/2024
using MefistoTheatreCompany.Models;
using MefistoTheatreCompany.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.Design;
using System.Web.Services.Description;
using System.Collections;
using Microsoft.Owin.Security.Provider;

namespace MefistoTheatreCompany.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : Controller
    {
        // GET: Moderator

        //here is the instance of the mefisto db contexxt
        private MefistoTheatreCompanyDbContext db = new MefistoTheatreCompanyDbContext();

        private UserManager<User> UserManager;

        public ModeratorController()
        {
            UserManager = new UserManager<User>(new UserStore<User>(db));
        }


        [Authorize(Roles = "Moderator")]
        public ActionResult Index()
        {

            return View();

        }

        public ActionResult ViewAllCategories()
        {
            //send a list of all the categories in the database to the view

            var categories = db.Categories.ToList();

            return View(categories);

        }


        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryViewModel model)
        {
            
                //make sure that there are no duplicate roles stored in the database
                if (db.Categories.Any(c => c.CategoryName == model.CategoryName))
                {

                    return RedirectToAction("CreateCategories", "Moderator");

                }
                else
                {
                //if no duplicate role present
                //add new category
                    db.Categories.Add(new Category
                    {
                        CategoryName = model.CategoryName,
                    });
                //save changes to the database
                    db.SaveChanges();
                    return RedirectToAction("ViewAllCategories", "Moderator");
                }
        }

        [HttpGet]
        public ActionResult EditCategory(string categoryName)
        {
            //get the category selected

            var category = db.Categories.SingleOrDefault(c => c.CategoryName == categoryName);

            if (category == null)
            {
                return HttpNotFound();
            }

            //return the category name to the view as the old category name

            return View(new CategoryViewModel
            {

                OldCategoryName = categoryName,
            });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> EditCategory(CategoryViewModel model, string categoryName)
        {

            if (ModelState.IsValid)
            {

                //retrieve category
                var category = db.Categories.SingleOrDefault(c => c.CategoryName == categoryName);


                //if no category is found
                if (category == null)
                {
                    return HttpNotFound();
                }

                //update the name
                category.CategoryName = model.NewCategoryName;

                //save changes to the database
                await db.SaveChangesAsync();

                return RedirectToAction("ViewAllCategories", "Moderator");


            }
            return View(model);

        }

        [HttpGet]
        public ActionResult DeleteCategory(string categoryName)
        {
            //get the category selected
            var category = db.Categories.SingleOrDefault(c => c.CategoryName == categoryName);

            if (category == null)
            {
                return HttpNotFound();
            }

            //return the category selected to the view when the page is loaded
            return View(new CategoryViewModel
            {

                CategoryName = category.CategoryName,
            });


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCategory(CategoryViewModel model, string categoryName)
        {
            //get the category using the category name selected
            var category = db.Categories.SingleOrDefault(c => c.CategoryName == categoryName);


            //if no category is found
            if (category == null)
            {
                return HttpNotFound();
            }


            //delete from database
            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return RedirectToAction("ViewAllCategories", "Moderator");


        }


        public ActionResult ViewAllPosts()
        {
            //get a list of approved and unapproved posts
            var ApprovedPosts = db.BlogPosts.Include("Category").Include("User").Where(p => p.Approved == true).OrderByDescending(p => p.DatePosted);
            var UnapprovedPosts = db.BlogPosts.Include("Category").Include("User").Where(p => p.Approved == false).OrderByDescending(p => p.DatePosted);
            //send the list of categories over the index page
            //so we can display them
            ViewBag.Categories = db.Categories.ToList();
            
            //send the lists of posts to the index page to be displayed
            ViewBag.ApprovedPosts = ApprovedPosts;
            ViewBag.UnapprovedPosts = UnapprovedPosts;

            return View();

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //search the posts table in the database
            //find post by id
            //return post
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.BlogPostId == id);

            if (blogPost == null)
            {
                return HttpNotFound();
            }

            //using the foreign key UserId from the post instance
            //find the suer who created the post
            var user = db.Users.Find(blogPost.UserId) as Staff;

            //using the foreign key category Id from these posts
            //find the category that the post belongs to
            var category = db.Categories.Find(blogPost.CategoryId);



            //assign the user to the User navigational property in Post
            blogPost.User = user;


            //send the post model to the Details View
            return View(blogPost);
        }


        public ActionResult EditPosts(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find post by Id in the posts table
            BlogPost post = db.BlogPosts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            //get a list of all the categories rom the table and send the list
            //to the view using a viewbag
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            var users = db.Users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList();
            ViewBag.Users = users;

            return View(post);
        }

        // POST: Staff/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPosts([Bind(Include = "BlogPostId,Title,Content,Approved,DatePosted,CategoryId")] BlogPost post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            if (ModelState.IsValid)
            {
                //set the new details to the blogpost

                BlogPost originalPost = db.BlogPosts.Find(post.BlogPostId);
                if (originalPost is null)
                {
                    HttpNotFound();
                }

                originalPost.Title = post.Title;
                originalPost.Content = post.Content;
                originalPost.DatePosted = db.BlogPosts.Where(p => p.BlogPostId == post.BlogPostId).Select(p => p.DatePosted).FirstOrDefault();
                originalPost.DateEdited = DateTime.Now;

                //if statement for if the Approved box has been ticked
                if (Request.Form["Approved"] == "true,false" || Request.Form["Approved"] == "true")
                {
                    originalPost.Approved = true;
                }
                else
                {
                    originalPost.Approved = false;
                }
                post.UserId = originalPost.UserId;

                //save edits to the database
                db.Entry(originalPost).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("ViewAllPosts");
            }
            //get a list of all the categories
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }



        public ActionResult DeletePost(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find the post 
            BlogPost post = db.BlogPosts.Find(id);

            var category = db.Categories.Find(post.CategoryId);

            post.Category = category;

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }


        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find the post by id
            BlogPost post = db.BlogPosts.Find(id);
            
            //delete from database

            db.BlogPosts.Remove(post);

            db.SaveChanges();

            return RedirectToAction("ViewAllPosts");


        }

        public ActionResult ViewAllComments()
        {
            //get a list of comments along with the user who created it and the blog the comment is of
            var comments = db.Comments.Include(p => p.User).Include(p => p.Blog).OrderByDescending(p => p.DatePosted);

            //send it to the view
            ViewBag.Comments = comments;

            return View();
        }
        public ActionResult CommentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get comment details by id
            Comment comment = db.Comments.FirstOrDefault(p => p.CommentId == id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            var user = db.Users.Find(comment.UserId);



            //assign the user to the User navigational property in comment
            comment.User = user;


            //send the comment model to the Details View
            return View(comment);
        }




        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get comment selected by id
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            //get the user who wrote the comment and the blog the comment is a part of
            var user = db.Users.Find(comment.UserId);
            var blog = db.BlogPosts.Find(comment.BlogId);

            var model = new DeleteCommentViewModel
            {
                //display the information of the comment on the view
                User = user,
                Blog = blog,
                CommentId = comment.CommentId.ToString(),
                IsSuspended = user.IsSuspended,
                DatePosted = comment.DatePosted,
            };



            return View(model);
        }



        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCommentConfirmed(int id, DeleteCommentViewModel model)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            var user = db.Users.Find(comment.UserId);

            if (user != null)
            {

                var isSuspended = model.IsSuspended;

                //if the user is now suspended
                if (isSuspended)
                {
                    var userManager = new UserManager<User>(new UserStore<User>(db));

                    if (isSuspended && !userManager.IsInRole(user.Id, "Suspended"))
                    {
                        string oldRole = (await userManager.GetRolesAsync(user.Id)).SingleOrDefault();

                        // Remove user from the old role first
                        if (!string.IsNullOrEmpty(oldRole))
                        {
                            await userManager.RemoveFromRoleAsync(user.Id, oldRole);
                        }

                        // Add the user to the new role
                        await userManager.AddToRoleAsync(user.Id, "Suspended");

                        await UserManager.UpdateAsync(user);
                    }

                }


            }
            db.Comments.Remove(comment);

            db.SaveChanges();



            return RedirectToAction("ViewAllComments");


        }


    }
}

