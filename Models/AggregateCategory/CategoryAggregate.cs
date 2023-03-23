using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinalEngineer.Models.AggregateCategory
{
    [Table("Category")]
    public class Category
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải có tên miền kiến thức")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên miền kiến thức")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Mô tả miền kiến thức")]
        public string Description { get; set; }

        public ICollection<Category> CategoryChildren { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        [Display(Name = "Danh mục cha")]
        public Category ParentCategory { set; get; }

    }
}