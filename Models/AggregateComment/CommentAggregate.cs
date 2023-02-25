using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateUser;

namespace ProjectFinalEngineer.Models.AggregateComment;

[Table("Comment")]
public class Comment
{
    [Key]
    public int Id { get; set; }
    public Post Post { get; set; }
    public AppUser Author { get; set; }
    public string Content { get; set; }
    public Comment Parent { get; set; }
    public DateTime CreatedOn { get; set; }
    public virtual IEnumerable<Comment> Comments { get; set; }

}