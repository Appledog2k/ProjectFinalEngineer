using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregateUser;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.RoomingHouse
{
    [Table("RoomingHouse")]
    public class RoomingHouse
    {
        [Key]
        public int Id { set; get; }

        [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Title { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        public float Price { set; get; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; } = false;
        public AppUser Approver { set; get; }

        public List<RommingHouseArea> RommingHouseAreas { get; set; }

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

        public long ViewCount { set; get; }
        public long ReactCount { set; get; }
        public int Priority { set; get; }
        // Lý do từ chối
        public string Reason { set; get; }
        public virtual IEnumerable<Comment> Comments { get; set; }

    }
}
