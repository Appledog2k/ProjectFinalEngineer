using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models.AggregateMessage;
using System.Collections.Generic;

namespace ProjectFinalEngineer.Models.AggregateRoom;
public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AppUser Admin { get; set; }
    public ICollection<Message> Messages { get; set; }

}