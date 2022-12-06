using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}