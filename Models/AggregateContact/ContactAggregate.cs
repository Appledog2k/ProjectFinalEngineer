using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinalEngineer.Models.AggregateContact;

public class Contact
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    [Required(ErrorMessage = "Phải nhập {0}")]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Phải nhập {0}")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Không đúng định dạng email.")]
    public string Email { get; set; }
    public DateTime? DateSent { get; set; }
    [Display(Name = "Nội dung.")]
    public string Message { get; set; }
    [StringLength(100)]
    [Display(Name = "Số điện thoại.")]
    public string Phone { get; set; }
}
