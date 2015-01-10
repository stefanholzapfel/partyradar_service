using System.ComponentModel.DataAnnotations;

namespace PartyService.ControllerModels
{
    public class ChangeLabel
    {
        [Required, Display(Name = "Label")]
        public string Label { get; set; }
    }

    public class AddLabel:ChangeLabel
    { }
}