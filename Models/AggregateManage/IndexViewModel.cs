using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateManage
{
    public class IndexViewModel
    {
        public EditExtraProfileModel Profile { get; set; }
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public string AuthenticatorKey { get; set; }
    }
}
