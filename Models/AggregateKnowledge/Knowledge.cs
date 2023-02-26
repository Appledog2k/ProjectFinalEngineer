using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateUser;

namespace ProjectFinalEngineer.Models.AggregateKnowledge
{
    [Table("Knowledge")]
    public class Knowledge
    {
        [Key]
        public int Id { set; get; }

        [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Title { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        public List<KnowledgeCategory> KnowledgeCategories { get; set; }

        [Display(Name = "Tác giả")]
        public string AuthorId { set; get; }
        [ForeignKey("AuthorId")]

        [Display(Name = "Tác giả")]
        public AppUser Author { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdated { set; get; }
    }
}
