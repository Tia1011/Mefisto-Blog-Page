// Tiani Perera - Mefisto Theatre Company 03/02/2024
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MefistoTheatreCompany.Models
{
    public class Staff : User
    {
        //Display Attributes
         public int? StaffId {  get; set; }
        //[Display(Name = "Role")]
        //public string Role { get; set; }
       
        public List<BlogPost> BlogPosts { get; set; }


    }

  

}