

namespace ProjectFinalEngineer.Services.Comment
{
    public interface ICommentService
    {
        Models.AggregateComment.Comment GetComment(int commentId);
        Task<Models.AggregateComment.Comment> Add(Models.AggregateComment.Comment comment);

    }
}