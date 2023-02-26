using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.RoomingHouse
{
    [Table("Area")]
        public class Area
        {

            [Key]
            public int Id { get; set; }
            // Tiều đề Category
            [Required(ErrorMessage = "Phải có tên danh mục")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
            [Display(Name = "Tên danh mục")]
            public string Title { get; set; }

            // Nội dung, thông tin chi tiết về Category
            [DataType(DataType.Text)]
            [Display(Name = "Nội dung danh mục")]
            public string Description { get; set; }

            // Các Category con
            public ICollection<Area> CategoryChildren { get; set; }

            // Category cha (FKey)
            [Display(Name = "Danh mục cha")]
            public int? ParentAreaId { get; set; }

            [ForeignKey("ParentCategoryId")]
            [Display(Name = "Danh mục cha")]
            public Area ParentArea { set; get; }

        }
}
