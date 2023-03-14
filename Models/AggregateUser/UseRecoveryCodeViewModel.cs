using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregateUser
{
    public class UseRecoveryCodeViewModel
    {

        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "Nhập mã phục hồi đã lưu")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}
