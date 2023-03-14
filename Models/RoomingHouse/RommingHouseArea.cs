using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinalEngineer.Models.RoomingHouse
{
    [Table("RommingHouseArea")]
    public class RommingHouseArea
    {
        public int AreaId { set; get; }

        public int RommingHouseId { set; get; }

        [ForeignKey("AreaID")]
        public Area Area { set; get; }

        [ForeignKey("RommingHouseID")]
        public RoomingHouse RoomingHouse { set; get; }
    }
}
