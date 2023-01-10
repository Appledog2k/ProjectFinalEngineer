using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregatePost
{
    public class CreatePostModel : Post
    {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }
    }
}