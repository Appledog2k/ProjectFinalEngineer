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

        [Required(ErrorMessage = "Phải có tiêu đề bài đăng")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Title { set; get; }

        [Display(Name = "Ảnh, Video")]
        [Required(ErrorMessage = "Phải có hình ảnh, video")]
        public string Image { set; get; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Phải có mô tả")]
        public string Description { set; get; }


        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        [Required(ErrorMessage = "Phải có giá tiền")]
        public float Price { set; get; } =0;

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; } = false;
        public AppUser Approver { set; get; }
        public List<RoomingHouseArea> RoomingHouseAreas { get; set; }

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
