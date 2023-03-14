using System.ComponentModel.DataAnnotations.Schema;
using ProjectFinalEngineer.Models.AggregateCategory;
using ProjectFinalEngineer.Models.AggregatePost;

namespace ProjectFinalEngineer.Models.AggregatePostCategory;
[Table("PostCategory")]
public class PostCategory
{
    public int PostId { set; get; }

    public int CategoryId { set; get; }

    [ForeignKey("PostId")]
    public Post Post { set; get; }

    [ForeignKey("CategoryId")]
    public Category Category { set; get; }
}