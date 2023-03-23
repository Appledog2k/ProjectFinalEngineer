using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateUser;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.BusinessManager;
using ProjectFinalEngineer.Models.AggregateExtensions;

namespace ProjectFinalEngineer.Controllers;

[Route("/forum/article/[action]/{id?}")]
[Authorize(Roles = RoleName.Administrator + "," + RoleName.Member)]
public class PostController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICommentBusinessManager _commentBusinessManager;
    public PostController(
        AppDbContext context,
        UserManager<AppUser> userManager,
        ICommentBusinessManager commentBusinessManager)
    {
        this._context = context;
        this._userManager = userManager;
        this._commentBusinessManager = commentBusinessManager;
    }

    [TempData]
    public string StatusMessage { get; set; }



    [AllowAnonymous]
    public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string searchString)
    {
        var posts = _context.Posts
            .Include(p => p.Author)
            .Include(post => post.Comments)
            .OrderByDescending(p => p.DateUpdated)
            .Where(x => x.Published == true);

        if (searchString != null)
        {
            posts = posts.Where(post => post.Title.ToLower().Contains(searchString.ToLower()) ||
                                        post.Content.Contains(searchString)
                                        || post.PostCategories
                                            .Any(pc => pc.Category.Title.ToLower().Contains(searchString.ToLower())));
        }
        
        var totalPosts = await posts.CountAsync();
        if (pagesize <= 0) pagesize = 10;

        var countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

        if (currentPage > countPages) currentPage = countPages;
        if (currentPage < 1) currentPage = 1;

        var pagingModel = new PagingModel()
        {
            CountPages = countPages,
            CurrentPage = currentPage,
            GenerateUrl = (pageNumber) => Url.Action("Index", new
            {
                p = pageNumber, pagesize
            })
        };

        ViewBag.pagingModel = pagingModel;
        ViewBag.totalPosts = totalPosts;

        ViewBag.postIndex = (currentPage - 1) * pagesize;

        var postsInPage = await posts.Skip((currentPage - 1) * pagesize)
                         .Take(pagesize)
                         .Include(p => p.PostCategories)
                         .ThenInclude(pc => pc.Category)
                         .ToListAsync();

        return View(postsInPage);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ListMyArticle([FromQuery(Name = "p")] int currentPage, int pagesize, string searchString)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var posts = _context.Posts
            .Include(p => p.Author)
            .Include(post => post.Comments)
            .OrderByDescending(p => p.DateUpdated)
            .Where(x => x.Author.Id == userId);

        if (searchString != null)
        {
            posts = posts.Where(post => post.Title.ToLower().Contains(searchString.ToLower()) ||
                                        post.Content.Contains(searchString)
                                        || post.PostCategories
                                            .Any(pc => pc.Category.Title.ToLower().Contains(searchString.ToLower())));
        }

        var totalPosts = await posts.CountAsync();
        if (pagesize <= 0) pagesize = 10;

        var countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

        if (currentPage > countPages) currentPage = countPages;
        if (currentPage < 1) currentPage = 1;

        var pagingModel = new PagingModel()
        {
            CountPages = countPages,
            CurrentPage = currentPage,
            GenerateUrl = (pageNumber) => Url.Action("Index", new
            {
                p = pageNumber,
                pagesize
            })
        };

        ViewBag.pagingModel = pagingModel;
        ViewBag.totalPosts = totalPosts;

        ViewBag.postIndex = (currentPage - 1) * pagesize;

        var postsInPage = await posts.Skip((currentPage - 1) * pagesize)
            .Take(pagesize)
            .Include(p => p.PostCategories)
            .ThenInclude(pc => pc.Category)
            .ToListAsync();

        return View(postsInPage);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Author)
            .Include(post => post.Comments)
                .ThenInclude(comment => comment.Author)
            .Include(post => post.Comments)
                .ThenInclude(comment => comment.Comments)
                    .ThenInclude(reply => reply.Parent)
            .FirstOrDefaultAsync(m => m.PostId == id);

        if (post == null)
        {
            return NotFound();
        }

        post.ViewCount++;
        _context.Update(post);
        await _context.SaveChangesAsync();

        var result = new PostViewModel
        {
            Post = post
        };
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ActionReact(int postId)
    {
        var post = _context.Posts.Find(postId);
        if (post != null)
        {
            post.ReactCount++;
        }

        _context.SaveChanges();
        return RedirectToAction("Details", "Post", new { id = postId });
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        var categories = await _context.Categories.ToListAsync();

        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Media,Content,CategoryIDs")] CreatePostModel post)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        if (!ModelState.IsValid) return View(post);

        var user = await _userManager.GetUserAsync(this.User);

        post.DateCreated = post.DateUpdated = DateTime.Now;
        post.ViewCount = 0;
        post.ReactCount = 0;
        post.AuthorId = user.Id;
        post.Published = false;
        post.Priority = 1;

        _context.Add(post);

        if (post.CategoryIDs != null)
        {
            foreach (var cateId in post.CategoryIDs)
            {
                _context.Add(new PostCategory()
                {
                    CategoryId = cateId,
                    Post = post
                });
            }
        }
        await _context.SaveChangesAsync();
        StatusMessage = "Vừa tạo bài viết, vui trong chờ quản trị viên phê duyệt";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts.Include(p => p.PostCategories)
        .Include(post => post.Author)
        .Include(post => post.Comments)
            .ThenInclude(comment => comment.Author)
        .Include(post => post.Comments)
            .ThenInclude(comment => comment.Comments)
                .ThenInclude(reply => reply.Parent)
        .FirstOrDefaultAsync(p => p.PostId == id);

        if (post == null)
        {
            return NotFound();
        }

        var postEdit = new CreatePostModel()
        {
            PostId = post.PostId,
            Title = post.Title,
            Media = post.Media,
            Content = post.Content,
            Published = false,
            CategoryIDs = post.PostCategories.Select(pc => pc.CategoryId).ToArray()
        };

        var categories = await _context.Categories.ToListAsync();
        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        return View(postEdit);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, [Bind("PostId,Title,Media,Content,CategoryIDs")] CreatePostModel post)
    {
        if (id != post.PostId)
        {
            return NotFound();
        }

        var categories = await _context.Categories.ToListAsync();
        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        if (ModelState.IsValid)
        {
            try
            {

                var postUpdate = await _context.Posts.Include(p => p.PostCategories).FirstOrDefaultAsync(p => p.PostId == id);
                if (postUpdate == null)
                {
                    return NotFound();
                }

                postUpdate.Title = post.Title;
                postUpdate.Content = post.Content;
                postUpdate.Media = post.Media;
                postUpdate.Published = false;
                postUpdate.DateUpdated = DateTime.Now;

                post.CategoryIDs ??= new int[] { };

                // Loại bỏ miền kiến thức cũ
                var oldCateIds = postUpdate.PostCategories.Select(c => c.CategoryId).ToArray();
                var newCateIds = post.CategoryIDs;

                var removeCatePosts = from postCate in postUpdate.PostCategories
                                      where (!newCateIds.Contains(postCate.CategoryId))
                                      select postCate;

                _context.PostCategories.RemoveRange(removeCatePosts);

                // Thêm miền kiến thức
                var addCateIds = from cateId in newCateIds
                                 where !oldCateIds.Contains(cateId)
                                 select cateId;

                foreach (var cateId in addCateIds)
                {
                    _context.PostCategories.Add(new PostCategory()
                    {
                        PostId = id,
                        CategoryId = cateId
                    });
                }

                _context.Update(postUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            StatusMessage = "Vừa cập nhật bài viết,vui lòng chờ quản trị viên xét duyệt";
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
        return View(post);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Comments)
            .Include(p => p.Author)
            .FirstOrDefaultAsync(m => m.PostId == id);

        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.Include(p => p.Comments)
            .Include(p => p.Author).FirstOrDefaultAsync(m => m.PostId == id);

        if (post == null)
        {
            return NotFound();
        }

        // Xóa comment liên quan tới vài viết
        _context.Comments.RemoveRange(post.Comments);

        // Xóa bài viết
        _context.Posts.Remove(post);

        await _context.SaveChangesAsync();
        StatusMessage = "Bạn vừa xóa bài viết: " + post.Title;

        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.PostId == id);
    }

    [HttpPost]
    public async Task<IActionResult> Comment(PostViewModel postViewModel)
    {
        var actionResult = await _commentBusinessManager.CreateComment(postViewModel, this.User);
        return actionResult.Result ?? RedirectToAction("Details", "Post", new { id = postViewModel.Post.PostId });
    }
}