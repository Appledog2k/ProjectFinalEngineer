using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string CurrentRoom { get; set; }
        public string Device { get; set; }
    }
}