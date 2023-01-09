using System.ComponentModel.DataAnnotations;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregatePost;

namespace ProjectFinalEngineer.Areas.Blog.Services
{
    public interface ICommentService
    {
        Comment GetComment(int commentId);
        Task<Comment> AddComment(Comment comment);

    }
}