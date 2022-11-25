using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models.AggregatePost;

namespace ProjectFinalEngineer.Areas.Blog.Models
{
    public class CreatePostModel : Post
    {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }
    }
}