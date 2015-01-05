using System.ComponentModel.DataAnnotations;
using PartyService.Models;

namespace PartyService.ControllerModels
{
    public class AddUser:RegisterBindingModel
    {
        [Required]
        [Display(Name = "IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}