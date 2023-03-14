using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateRole
{
    public class RoleModel : IdentityRole
    {
        public string[] Claims { get; set; }
    }
}
