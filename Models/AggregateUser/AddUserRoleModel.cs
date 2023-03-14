using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateUser
{
    public class AddUserRoleModel
    {
        public AppUser User { get; set; }

        [DisplayName("Các role gán cho user")]
        public string[] RoleNames { get; set; }
        public List<IdentityRoleClaim<string>> ClaimsInRole { get; set; }
        public List<IdentityUserClaim<string>> ClaimsInUserClaim { get; set; }

    }
}