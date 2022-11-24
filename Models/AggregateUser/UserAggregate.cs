using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateContact;

public class UserAggregate : IdentityUser
{
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }
}
