// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class EditCommentViewModel
    {
        //attributes used when a comment is being edited
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        
        public DateTime DatePosted { get; set; }
        public int BlogPostId { get; set; }
    }
}