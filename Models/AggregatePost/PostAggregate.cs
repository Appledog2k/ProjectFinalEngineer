using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateUser;

namespace ProjectFinalEngineer.Models.AggregatePost;

[Table("Post")]
public class Post
{
    [Key]
    public int PostId { set; get; }

    [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
    [Display(Name = "Tiêu đề")]
    [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
    public string Title { set; get; }

    [Display(Name = "Nội dung")]
    public string Content { set; get; }

    [Display(Name = "Xuất bản")]
    public bool Published { set; get; }

    public List<PostCategory> PostCategories { get; set; }

    // [Required]
    [Display(Name = "Tác giả")]
    public string AuthorId { set; get; }
    [ForeignKey("AuthorId")]
    [Display(Name = "Tác giả")]
    public AppUser Author { set; get; }



    [Display(Name = "Ngày tạo")]
    public DateTime DateCreated { set; get; }

    [Display(Name = "Ngày cập nhật")]
    public DateTime DateUpdated { set; get; }
    public AppUser Approver { set; get; }
    public virtual IEnumerable<Comment> Comments { get; set; }

}