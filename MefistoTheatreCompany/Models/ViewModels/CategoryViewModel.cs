// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class CategoryViewModel
    {
        //attributes used and displayed when updating category
        [Required(ErrorMessage ="CategoryName is required")]
        public string CategoryName { get; set; }
        public string OldCategoryName { get; set; }

        [Required]
        public string NewCategoryName { get; set; }
    }
}