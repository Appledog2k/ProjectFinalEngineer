using App.Models.AggregateExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.Models.AggregateUser;
using ProjectFinalEngineer.Models.RoomingHouse;

namespace ProjectFinalEngineer.Controllers
{
    [Route("/forum/roominghouse/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Member)]
    public class RoomingHouseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RoomingHouseController(AppDbContext context, UserManager<AppUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        // GET: Blog/Post
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string searchString = null)
        {
            var roomingHouses = _context.RoomingHouses
                .OrderByDescending(p => p.DateUpdated)
                .Include(p => p.Author)
                .Include(post => post.Comments);
                //.Where(post => post.Title.Contains(searchString) || post.Content.Contains(searchString));


            int totalPosts = await roomingHouses.CountAsync();
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

            var roomingHousesInPage = await roomingHouses.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .Include(p => p.RommingHouseAreas)
                .ThenInclude(pc => pc.Area)
                .ToListAsync();

            return View(roomingHousesInPage);
        }

        // GET: Blog/Post/Create
        public async Task<IActionResult> CreateAsync()
        {
            var areas = await _context.Areas.ToListAsync();

            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Price,Published,CategoryIDs")] CreateRoomingHouseModel roomingHouse)
        {
            var areas = await _context.Areas.ToListAsync();
            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(this.User);
                roomingHouse.DateCreated = roomingHouse.DateUpdated = DateTime.Now;
                roomingHouse.AuthorId = user.Id;
                _context.Add(roomingHouse);

                if (roomingHouse.AreaIDs != null)
                {
                    foreach (var AreaId in roomingHouse.AreaIDs)
                    {
                        _context.Add(new RommingHouseArea()
                        {
                            AreaID = AreaId,
                            RoomingHouse = roomingHouse
                        });
                    }
                }
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa đăng tin";
                return RedirectToAction(nameof(Index));
            }
            return View(roomingHouse);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomingHouse = await _context.RoomingHouses
                .Include(p => p.Author)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.Author)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.Comments)
                .ThenInclude(reply => reply.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (roomingHouse == null)
            {
                return NotFound();
            }

            var result = new RoomingHouseViewModel()
            {
                RoomingHouse = roomingHouse
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
            var roomingHouse = await _context.RoomingHouses.Include(p => p.RommingHouseAreas)
                .Include(post => post.Author)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.Author)
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.Comments)
                .ThenInclude(reply => reply.Parent)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (roomingHouse == null)
            {
                return NotFound();
            }

            var roomingHouseEdit = new CreateRoomingHouseModel()
            {
                Id = roomingHouse.Id,
                Title = roomingHouse.Title,
                Content = roomingHouse.Content,
                Price = roomingHouse.Price,
                Published = false,
                AreaIDs = roomingHouse.RommingHouseAreas.Select(pc => pc.AreaID).ToArray()
            };

            var areas = await _context.Areas.ToListAsync();
            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            return View(roomingHouseEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Price,AreaIDs")] CreateRoomingHouseModel roomingHouse)
        {
            if (id != roomingHouse.Id)
            {
                return NotFound();
            }
            var areas = await _context.Areas.ToListAsync();
            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");
            if (ModelState.IsValid)
            {
                try
                {

                    var roomingHousesUpdate = await _context.RoomingHouses.Include(p => p.RommingHouseAreas).FirstOrDefaultAsync(p => p.Id == id);
                    if (roomingHousesUpdate == null)
                    {
                        return NotFound();
                    }

                    roomingHousesUpdate.Title = roomingHouse.Title;
                    roomingHousesUpdate.Content = roomingHouse.Content;
                    roomingHousesUpdate.Published = roomingHouse.Published;
                    roomingHousesUpdate.DateUpdated = DateTime.Now;
                    roomingHousesUpdate.Price = roomingHouse.Price;

                    // Update PostCategory
                    roomingHouse.AreaIDs ??= new int[] { };

                    var oldAreaIds = roomingHousesUpdate.RommingHouseAreas.Select(c => c.AreaID).ToArray();
                    var newAreaIds = roomingHouse.AreaIDs;

                    var removeRoomingHousesAreas = from roomingHousesAreas in roomingHousesUpdate.RommingHouseAreas
                                          where (!newAreaIds.Contains(roomingHousesAreas.AreaID))
                                          select roomingHousesAreas;
                    _context.RommingHouseAreas.RemoveRange(removeRoomingHousesAreas);

                    var addRoomingHousesAreas = from areaId in newAreaIds
                                     where !oldAreaIds.Contains(areaId)
                                     select areaId;

                    foreach (var areasId in addRoomingHousesAreas)
                    {
                        _context.RommingHouseAreas.Add(new RommingHouseArea()
                        {
                            RommingHouseID = id,
                            AreaID = areasId
                        });
                    }

                    _context.Update(roomingHousesUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomingHouseExists(roomingHouse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Vừa cập nhật bản tin";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", roomingHouse.AuthorId);
            return View(roomingHouse);
        }

        private bool RoomingHouseExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.RoomingHouses
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
            var post = await _context.RoomingHouses.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.RoomingHouses.Remove(post);
            await _context.SaveChangesAsync();

            StatusMessage = "Bạn vừa xóa bài viết: " + post.Title;

            return RedirectToAction(nameof(Index));
        }

    }
}
