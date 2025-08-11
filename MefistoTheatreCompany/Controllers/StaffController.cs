// Tiani Perera - Mefisto Theatre Company 03/02/2024
using MefistoTheatreCompany.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Permissions;
using System.EnterpriseServices;
using System.Web.Services.Description;
using System.Data;
using System.Net;

namespace MefistoTheatreCompany.Controllers
{
    public class StaffController : Controller
    { 
        //instance of a database
        private MefistoTheatreCompanyDbContext context = new MefistoTheatreCompanyDbContext();


        [Authorize(Roles = "Admin,Employee")]
        
        public ActionResult Index()
        {
            //select all the posts from the posts table
            //including the foreign kets category and user and comments

            var blogPosts = context.BlogPosts.Include(p => p.Category).Include(p => p.User).Include(p => p.Comments.Select(q => q.User)).OrderByDescending(p => p.DatePosted);


            //get the id of the logged in user
            var userId = User.Identity.GetUserId();

            //from the list of posts
            //select only the ones that have the userID equal to th ID of the logged in user
            //returns a list of posts
            blogPosts = blogPosts.Where(p => p.UserId == userId).OrderByDescending(p=>p.DatePosted);
            return View(blogPosts.ToList());
        }

        
          
         

        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            //finds blog post using the id and get it
            var blogPost = context.BlogPosts.Include(p => p.Category).Include(p => p.User).Include(p => p.Comments.Select(q => q.User)).FirstOrDefault(p=>p.BlogPostId==id);


           
            //if blogpost not found
            if (blogPost == null)
            {
                return HttpNotFound();
            }

            //returns the blog post to the view
            return View(blogPost);

         


        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            //send the list of categories to the view using viewbag
            //so user can select the category for the post from a dropdown box
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Staff/Create
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content,CategoryId")] BlogPost post)
        {


            if (ModelState.IsValid)
            {
                //get the values in the form and create a new post
                post.DatePosted = DateTime.Now;
                post.DateEdited= DateTime.Now;

                post.UserId = User.Identity.GetUserId();

                context.BlogPosts.Add(post);

                context.SaveChanges();

                return RedirectToAction("Index");
            }

            //if model is not valid get the list of categories and display the page 
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find post by Id in the posts table
            BlogPost post = context.BlogPosts.Find(id); 

            if (post == null)
            {
                return HttpNotFound();
            }

            //get a list of all the categories from the table and send the list
            //to the view using a viewbag
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // POST: Staff/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "BlogPostId,Title,Content,DatePosted,CategoryId")] BlogPost post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            if (ModelState.IsValid)
            {
                //ammend the post details to the ones in the form
                post.DatePosted = context.BlogPosts.Where(p => p.BlogPostId == post.BlogPostId).Select(p => p.DatePosted).FirstOrDefault();
                post.DateEdited = DateTime.Now;


                post.UserId = User.Identity.GetUserId();

                //save the changes to the database

                context.Entry(post).State = EntityState.Modified;

                context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find the blog post using its id
            BlogPost post = context.BlogPosts.Find(id);

            //find the category for that blog post
            var category = context.Categories.Find(post.CategoryId);

            post.Category = category;

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find the blog post by its id

            BlogPost post = context.BlogPosts.Find(id);
            //delete it from the database and save the changes

            context.BlogPosts.Remove(post);

            context.SaveChanges();

            return RedirectToAction("Index");


        }
    }
}
