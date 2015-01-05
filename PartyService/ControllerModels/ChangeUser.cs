using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PartyService.Models;

namespace PartyService.ControllerModels
{
    public class ChangeUser
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth date")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Gender")]
        public GenderType? Gender { get; set; }

        [Display(Name = "IsAdmin")]
        public bool? IsAdmin { get; set; }
    }
}