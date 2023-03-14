using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.RoomingHouse
{
    public class CreateRoomingHouseModel : RoomingHouse
    {
        [Display(Name = "Khu vực")]
        public int[] AreaIDs { get; set; }
    }
}
