// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MefistoTheatreCompany.Models
{
    public class Category
    {
        //create attributes
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        //navigational property
        public List<BlogPost> BlogPosts { get; set; }

    }
}