using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregateManage
{
    public class DisplayRecoveryCodesViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }
    }
}
