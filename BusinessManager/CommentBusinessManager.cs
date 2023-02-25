using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregateUser;
using ProjectFinalEngineer.Services.Comment;
using System.Security.Claims;

namespace ProjectFinalEngineer.BusinessManager
{
    public class CommentBusinessManager : ICommentBusinessManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommentService _commentService;
        private readonly AppDbContext _context;

        public CommentBusinessManager(UserManager<AppUser> userManager, ICommentService commentService, AppDbContext context)
        {
            _userManager = userManager;
            _commentService = commentService;
            _context = context;
        }

        public async Task<ActionResult<Comment>> CreateComment(PostViewModel postViewModel, ClaimsPrincipal claimsPrincipal)
        {
            if (postViewModel.Post is null || postViewModel.Post.PostId == 0)
                return new BadRequestResult();
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.Author)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.Comments)
                .ThenInclude(reply => reply.Parent)
                .FirstOrDefaultAsync(m => m.PostId == postViewModel.Post.PostId);

            if (post is null)
                return new NotFoundResult();


            var comment = postViewModel.Comment;
            comment.Author = await _userManager.GetUserAsync(claimsPrincipal);
            comment.Post = post;
            comment.CreatedOn = DateTime.Now;
            if (comment.Parent != null)
            {
                comment.Parent = _commentService.GetComment(comment.Parent.Id);
            }

            return await _commentService.Add(comment);
        }
    }
}
