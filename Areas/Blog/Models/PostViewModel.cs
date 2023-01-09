using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregatePost;

namespace ProjectFinalEngineer.Areas.Blog.Models
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }
}