 using Microsoft.EntityFrameworkCore;
 using ProjectFinalEngineer.EntityFramework;

 namespace ProjectFinalEngineer.Services.Comment
 {
     public class CommentService : ICommentService
     {
         private readonly AppDbContext _context;
         public CommentService(AppDbContext context)
         {
             _context = context;
         }

         public Models.AggregateComment.Comment GetComment(int commentId)
         {
             return _context.Comments
                 .Include(comment => comment.Author)
                 .Include(comment => comment.Post)
                 .Include(comment => comment.Parent)
                 .FirstOrDefault(comment => comment.Id == commentId);
        }

         public async Task<Models.AggregateComment.Comment> Add(Models.AggregateComment.Comment comment)
         {
             _context.Add(comment);
             await _context.SaveChangesAsync();
             return comment;
        }
     }
 }