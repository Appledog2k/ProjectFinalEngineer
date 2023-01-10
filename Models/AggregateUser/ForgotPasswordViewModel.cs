
using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregateUser
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
