using App.Models.AggregateExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateKnowledge;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.Models.AggregateUser;

namespace ProjectFinalEngineer.Controllers
{
    [Route("/forum/knowledge/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Member)]
    public class KnowledgeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public KnowledgeController(AppDbContext context, UserManager<AppUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: forum/knowledge
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string searchString = null)
        {
            var knowledges = _context.Knowledges
                .OrderByDescending(p => p.DateUpdated)
                .Include(p => p.Author);
            //.Where(post => post.Title.Contains(searchString) || post.Content.Contains(searchString));


            int totalPosts = await knowledges.CountAsync();
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

            var knowledgesInPage = await knowledges.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .Include(p => p.KnowledgeCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync();

            return View(knowledgesInPage);
        }

        // GET: Blog/Post/Create
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,CategoryIDs")] CreateKnowledgeModel knowledge)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(this.User);
                knowledge.DateCreated = knowledge.DateUpdated = DateTime.Now;
                knowledge.AuthorId = user.Id;
                _context.Add(knowledge);

                if (knowledge.CategoryIDs != null)
                {
                    foreach (var CateId in knowledge.CategoryIDs)
                    {
                        _context.Add(new KnowledgeCategory()
                        {
                            CategoryID = CateId,
                            Knowledge = knowledge
                        });
                    }
                }
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo bài viết mới";
                return RedirectToAction(nameof(Index));
            }
            return View(knowledge);
        }

        // GET: Blog/Post/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knowledge = await _context.Knowledges
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (knowledge == null)
            {
                return NotFound();
            }

            var result = new KnowledgeViewModel()
            {
                Knowledge = knowledge
            };
            return View(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var post = await _context.Posts.FindAsync(id);
            var knowledge = await _context.Knowledges.Include(p => p.KnowledgeCategories)
                .Include(post => post.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (knowledge == null)
            {
                return NotFound();
            }

            var knowledgeEdit = new CreateKnowledgeModel()
            {
                Id = knowledge.Id,
                Title = knowledge.Title,
                Content = knowledge.Content,
                CategoryIDs = knowledge.KnowledgeCategories.Select(pc => pc.CategoryID).ToArray()
            };

            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            return View(knowledgeEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Content,CategoryIDs")] CreateKnowledgeModel knowledge)
        {
            if (id != knowledge.Id)
            {
                return NotFound();
            }
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            if (ModelState.IsValid)
            {
                try
                {

                    var knowledgeUpdate = await _context.Knowledges
                        .Include(p => p.KnowledgeCategories)
                        .FirstOrDefaultAsync(p => p.Id == id);
                    if (knowledgeUpdate == null)
                    {
                        return NotFound();
                    }

                    knowledgeUpdate.Title = knowledge.Title;
                    knowledgeUpdate.Content = knowledge.Content;
                    knowledgeUpdate.DateUpdated = DateTime.Now;

                    // Update PostCategory
                    knowledge.CategoryIDs ??= new int[] { };

                    var oldCateIds = knowledgeUpdate.KnowledgeCategories.Select(c => c.CategoryID).ToArray();
                    var newCateIds = knowledge.CategoryIDs;

                    var removeCatePosts = from knowledgeCate in knowledgeUpdate.KnowledgeCategories
                                          where (!newCateIds.Contains(knowledgeCate.CategoryID))
                                          select knowledgeCate;
                    _context.KnowledgeCategories.RemoveRange(removeCatePosts);

                    var addCateIds = from CateId in newCateIds
                                     where !oldCateIds.Contains(CateId)
                                     select CateId;

                    foreach (var CateId in addCateIds)
                    {
                        _context.KnowledgeCategories.Add(new KnowledgeCategory()
                        {
                            KnowledgeID = id,
                            CategoryID = CateId
                        });
                    }

                    _context.Update(knowledgeUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnowledgeExists(knowledge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Vừa cập nhật kiến thức";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", knowledge.AuthorId);
            return View(knowledge);
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Knowledges
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var knowledge = await _context.Knowledges.FindAsync(id);

            if (knowledge == null)
            {
                return NotFound();
            }

            _context.Knowledges.Remove(knowledge);
            await _context.SaveChangesAsync();

            StatusMessage = "Bạn vừa xóa kiến thức: " + knowledge.Title;

            return RedirectToAction(nameof(Index));
        }
        private bool KnowledgeExists(int id)
        {
            return _context.Knowledges.Any(e => e.Id == id);
        }


    }
}
