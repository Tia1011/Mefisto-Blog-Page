// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System;
using System.Security.Permissions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace MefistoTheatreCompany.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public abstract class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


       
       //Display Attributed

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }

        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }


        [Display(Name = "Joined")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM//dd/yyyy}")]
        public DateTime RegisteredAt { get; set; }

        public List<Comment> Comments { get; set; }

        //needing the ApplicationUserManager to get the users current role
        private ApplicationUserManager userManager;



        [NotMapped]
        public string CurrentRole
        {
            get
            {
                if (userManager == null)
                {
                    //initialize  userManager
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return userManager.GetRoles(Id).Single();
            }
        }
       
    }


}

    
