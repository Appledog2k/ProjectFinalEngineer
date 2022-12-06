using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Models
{
    public class MessageViewModel
    {
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string From { get; set; }
        [Required]
        public string Room { get; set; }
        public string Avatar { get; set; }
    }
}