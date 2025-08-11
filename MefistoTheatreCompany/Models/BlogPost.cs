// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MefistoTheatreCompany.Models
{
    public class BlogPost
    {
        //create attributes
        [Key]
        public int BlogPostId { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public bool Approved { get; set; }

        [Required]

        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode =true)] //format as short date time
        public DateTime DatePosted { get; set; }


        [Display(Name = "Date Edited")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] //format as short date time
    
        public DateTime? DateEdited { get; set; }


        //NAvigational properties
        [ForeignKey("User")]
        public string UserId { get; set; }
        public Staff User { get; set; }


        //navigation property
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }

    }
}