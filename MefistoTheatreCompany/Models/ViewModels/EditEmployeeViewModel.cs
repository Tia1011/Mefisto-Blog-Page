// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MefistoTheatreCompany.Models.ViewModels
{
    public class EditEmployeeViewModel
    {
        //attributes used when an employee is being edited

        [Required]
        [Display(Name = "StaffId")]
        public int StaffId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

      
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]



        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirm { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }


        [Display(Name = "Joined")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM//dd/yyyy}")]
        public DateTime RegisteredAt { get; set; }


        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }


        //public string Role { get; set; }
        [Display(Name = "Role")]
         public IEnumerable<SelectListItem> Roles { get; set; }



    }
}