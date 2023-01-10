using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateUser
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "varchar")]
        [StringLength(400)]
        public string HomeAdress { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }
}
