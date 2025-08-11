// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class CreateEmployeeViewModel
    {
        ////attributes used and displayed when creating staff
        //[Required,Display(Name = "StaffId")]
        //public int StaffId { get; set; }

        [Required,Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required,Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required]
        public string Street { get; set; }
        public string City { get; set; }

        [DataType(DataType.PostalCode)]

        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required,Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirm { get; set; }

        [Required, Display(Name = "Password")]
        public string Password { get; set; }

               

        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }



        

        [Display(Name = "Role")]
        public string Role { get; set; }
        

        public IEnumerable<SelectListItem> Roles { get; set; }


//icollection

    }
}