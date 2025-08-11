// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class DeleteCommentViewModel
    {
        //attributes used when deleting a comment
        public User User { get; set; }
        public BlogPost Blog { get; set; }
        public string CommentId { get; set; }
        public bool IsSuspended { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

    }
}