// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class DeleteViewModel
    {
        //attributes used when deleting a user
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string Street { get; set; }
        public string City { get; set; }

        [DataType(DataType.PostalCode)]

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirm { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        //{0:MM/dd/YYYY}

        [Display(Name = "Joined")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime RegisteredAt { get; set; }



        [Display(Name = "Role")]
        public string Role { get; set; }
        // public Role SelectedRole { get; set; }
        public ICollection<SelectListItem> Roles { get; set; }


    }
}
