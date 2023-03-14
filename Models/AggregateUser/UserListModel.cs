namespace ProjectFinalEngineer.Models.AggregateUser
{
    public class UserListModel
    {
        public int TotalUsers { get; set; }
        public int CountPages { get; set; }
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; }
        public List<UserAndRole> Users { get; set; }
    }
    public class UserAndRole : AppUser
    {
        public string RoleNames { get; set; }
    }
}