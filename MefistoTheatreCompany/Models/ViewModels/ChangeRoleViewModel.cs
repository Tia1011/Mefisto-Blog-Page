// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class ChangeRoleViewModel
    {
        //attributes used and displayed when changing roles
        public string Username { get;set; }
       public string OldRole { get;set; }
       public ICollection<SelectListItem> Roles { get; set; }

        [Required,Display(Name ="Role")]
        public string Role { get; set; }

        //[Display(Name = "Suspend")]
        //public bool IsSuspended {  get; set; }
    }
}