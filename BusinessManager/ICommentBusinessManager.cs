using Microsoft.AspNetCore.Mvc;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregatePost;
using System.Security.Claims;

namespace ProjectFinalEngineer.BusinessManager
{
    public interface ICommentBusinessManager
    {
        Task<ActionResult<Comment>> CreateComment(PostViewModel postViewModel, ClaimsPrincipal claimsPrincipal);
    }
}
