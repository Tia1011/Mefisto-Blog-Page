// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class EditRoleViewModel
    {
       //attributes used when a users role is being changed

        [Display(Name = "Role")]
        public string RoleName { get; set; }    
        public string OldRoleName { get; set; }

        [Required]
        public string NewRoleName { get; set; }
    }
}