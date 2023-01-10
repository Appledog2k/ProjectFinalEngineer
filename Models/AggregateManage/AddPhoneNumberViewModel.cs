using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregateManage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}
