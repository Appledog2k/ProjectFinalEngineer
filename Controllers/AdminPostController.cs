using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateExtensions;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateRole;

namespace ProjectFinalEngineer.Controllers
{
    [Route("/forum/AdminPost/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Member)]
    public class AdminPostController : Controller
    {
        private readonly AppDbContext _context;
        public AdminPostController(AppDbContext context)
        {
            _context = context;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string searchString = null)
        {
            var posts = _context.Posts
                .OrderBy(p => p.Priority)
                .ThenByDescending(p => p.DateUpdated)
                .Include(p => p.Author)
                .Include(post => post.Comments)
                .Where(x => x.Published == false);

            if (searchString != null)
            {
                posts = posts.Where(post => post.Title.Contains(searchString) || post.Content.Contains(searchString));
            }

            int totalPosts = await posts.CountAsync();
            if (pagesize <= 0) pagesize = 10;
            int countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

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

            var result = new PostViewModel
            {
                Post = post
            };
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovePost(int postId)
        {
            var post = _context.Posts.Find(postId);
            if (post != null)
            {
                post.Published = true;
                post.DateUpdated = DateTime.Now;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Refuse(int? id)
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
                Reason = post.Reason,
                Title = post.Title,
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
        public async Task<IActionResult> Refuse(int id, [Bind("PostId,Title,Reason,CategoryIDs")] CreatePostModel post)
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
                    postUpdate.DateUpdated = DateTime.Now;
                    postUpdate.Reason = post.Reason;
                    postUpdate.Priority = 2;
       
                    // Update PostCategory
                    post.CategoryIDs ??= new int[] { };

                    var oldCateIds = postUpdate.PostCategories.Select(c => c.CategoryId).ToArray();
                    var newCateIds = post.CategoryIDs;

                    var removeCatePosts = from postCate in postUpdate.PostCategories
                                          where (!newCateIds.Contains(postCate.CategoryId))
                                          select postCate;
                    _context.PostCategories.RemoveRange(removeCatePosts);

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
                StatusMessage = "Vừa cập nhật bài viết";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }
        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
