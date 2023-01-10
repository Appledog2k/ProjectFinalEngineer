using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateManage
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationScheme> OtherLogins { get; set; }
    }
}
