// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MefistoTheatreCompany.Models
{
    public class Comment
    {

        //create attributes
        [Key]
        public int CommentId { get; set; }

        

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public bool Approved { get; set; }

        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]//format as short date time
        public DateTime DatePosted { get; set; }


        [Display(Name = "Date Edited")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] //format as short date time
        public DateTime? DateEdited { get; set; }



        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public BlogPost Blog { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}