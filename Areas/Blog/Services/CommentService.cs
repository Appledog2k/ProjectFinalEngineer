using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregatePost;

namespace ProjectFinalEngineer.Areas.Blog.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        public CommentService(AppDbContext context)
        {
            _context = context;
        }
        public Comment GetComment(int commentId)
        {
            return _context.Comments
                .Include(comment => comment.Author)
                .Include(comment => comment.Post)
                .Include(comment => comment.Parent)
                .FirstOrDefault(comment => comment.Id == commentId);
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}