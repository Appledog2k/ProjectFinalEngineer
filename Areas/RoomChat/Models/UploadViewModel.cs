using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Models
{
    public class UploadViewModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}