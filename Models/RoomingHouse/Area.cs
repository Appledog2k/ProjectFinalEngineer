using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.RoomingHouse
{
    [Table("Area")]
    public class Area
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải có tên khu vực")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên khu vực")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Mô tả ngắn gọn")]
        public string Description { get; set; }

        public ICollection<Area> AreaChildren { get; set; }

        [Display(Name = "Khu vực cha")]
        public int? ParentAreaId { get; set; }

        [ForeignKey("ParentAreaId")]
        [Display(Name = "Khu vực cha")]
        public Area ParentArea { set; get; }

    }
}
