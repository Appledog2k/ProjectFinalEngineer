using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinalEngineer.Models.RoomingHouse
{
    [Table("RoomingHouseArea")]
    public class RoomingHouseArea
    {
        public int RoomingHouseId { set; get; }
        public int AreaId { set; get; }

        [ForeignKey("AreaId")]
        public Area Area { set; get; }

        [ForeignKey("RoomingHouseId")]
        public RoomingHouse RoomingHouse { set; get; }
    }
}
