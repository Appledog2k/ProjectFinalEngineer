using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using App.Utilities;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateUser;
using App.Models.AggregateExtensions;
using ProjectFinalEngineer.Models.AggregatePost;

namespace ProjectFinalEngineer.Controllers;

[Route("/forum/article/[action]/{id?}")]
[Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
public class PostController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public PostController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [TempData]
    public string StatusMessage { get; set; }
    // GET: Blog/Post
    [AllowAnonymous]
    public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
    {
        var posts = _context.Posts
                    .Include(p => p.Author)
                    .OrderByDescending(p => p.DateUpdated);

        int totalPosts = await posts.CountAsync();
        if (pagesize <= 0) pagesize = 10;
        int countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

        if (currentPage > countPages) currentPage = countPages;
        if (currentPage < 1) currentPage = 1;

        var pagingModel = new PagingModel()
        {
            countpages = countPages,
            currentpage = currentPage,
            generateUrl = (pageNumber) => Url.Action("Index", new
            {
                p = pageNumber,
                pagesize = pagesize
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

    // GET: Blog/Post/Details/5
    [AllowAnonymous]

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(m => m.PostId == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // GET: Blog/Post/Create
    public async Task<IActionResult> CreateAsync()
    {
        var categories = await _context.Categories.ToListAsync();

        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        return View();
    }

    // POST: Blog/Post/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        if (ModelState.IsValid)
        {

            var user = await _userManager.GetUserAsync(this.User);
            post.DateCreated = post.DateUpdated = DateTime.Now;
            post.AuthorId = user.Id;
            _context.Add(post);

            if (post.CategoryIDs != null)
            {
                foreach (var CateId in post.CategoryIDs)
                {
                    _context.Add(new PostCategory()
                    {
                        CategoryID = CateId,
                        Post = post
                    });
                }
            }
            await _context.SaveChangesAsync();
            StatusMessage = "Vừa tạo bài viết mới";
            return RedirectToAction(nameof(Index));
        }


        return View(post);
    }

    // GET: Blog/Post/Edit/5

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // var post = await _context.Posts.FindAsync(id);
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
            Content = post.Content,
            Published = post.Published,
            CategoryIDs = post.PostCategories.Select(pc => pc.CategoryID).ToArray()
        };

        var categories = await _context.Categories.ToListAsync();
        ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

        return View(postEdit);
    }

    // POST: Blog/Post/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post)
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
                postUpdate.Published = post.Published;
                postUpdate.DateUpdated = DateTime.Now;

                // Update PostCategory
                if (post.CategoryIDs == null) post.CategoryIDs = new int[] { };

                var oldCateIds = postUpdate.PostCategories.Select(c => c.CategoryID).ToArray();
                var newCateIds = post.CategoryIDs;

                var removeCatePosts = from postCate in postUpdate.PostCategories
                                      where (!newCateIds.Contains(postCate.CategoryID))
                                      select postCate;
                _context.PostCategories.RemoveRange(removeCatePosts);

                var addCateIds = from CateId in newCateIds
                                 where !oldCateIds.Contains(CateId)
                                 select CateId;

                foreach (var CateId in addCateIds)
                {
                    _context.PostCategories.Add(new PostCategory()
                    {
                        PostID = id,
                        CategoryID = CateId
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
            StatusMessage = "Vừa cập nhật bài viết";
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
        return View(post);
    }

    // GET: Blog/Post/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(m => m.PostId == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // POST: Blog/Post/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        StatusMessage = "Bạn vừa xóa bài viết: " + post.Title;

        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.PostId == id);
    }
}