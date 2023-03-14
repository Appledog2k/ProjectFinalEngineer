using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregatePost
{
    public class CreatePostModel : Post
    {
        [Display(Name = "Miền kiến thức")]
        public int[] CategoryIDs { get; set; }
    }
}