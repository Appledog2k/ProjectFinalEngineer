using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateExtensions;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.Models.AggregateUser;
using ProjectFinalEngineer.Models.RoomingHouse;

namespace ProjectFinalEngineer.Controllers
{
    [Route("/RoomingHouse/[action]/{id?}")]
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

        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string searchString = null)
        {
            var roomingHouses = _context.RoomingHouses
                .OrderByDescending(p => p.DateUpdated)
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Where(x => x.Published == true);

            if (searchString != null)
            {
                roomingHouses = roomingHouses.Where(post => post.Title.ToLower().Contains(searchString.ToLower()) || post.Content.Contains(searchString));
            }


            var totalPosts = await roomingHouses.CountAsync();

            if (pagesize <= 0) pagesize = 5;
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

            var roomingHousesInPage = await roomingHouses.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .Include(p => p.RoomingHouseAreas)
                .ThenInclude(pc => pc.Area)
                .ToListAsync();

            return View(roomingHousesInPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionReact(int postId)
        {
            var roomingHouse = _context.RoomingHouses.Find(postId);
            if (roomingHouse != null)
            {
                roomingHouse.ReactCount++;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateAsync()
        {
            var areas = await _context.Areas.ToListAsync();

            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Content,Price,Image,AreaIDs")] CreateRoomingHouseModel roomingHouse)
        {
            var areas = await _context.Areas.ToListAsync();
            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            if (!ModelState.IsValid) return View(roomingHouse);

            var user = await _userManager.GetUserAsync(this.User);

            roomingHouse.DateCreated = roomingHouse.DateUpdated = DateTime.Now;
            roomingHouse.ViewCount = 0;
            roomingHouse.ReactCount = 0;
            roomingHouse.AuthorId = user.Id;
            roomingHouse.Published = false;
            roomingHouse.Priority = 1;

            _context.Add(roomingHouse);

            if (roomingHouse.AreaIDs != null)
            {
                foreach (var areaId in roomingHouse.AreaIDs)
                {
                    _context.Add(new RoomingHouseArea()
                    {
                        AreaId = areaId,
                        RoomingHouse = roomingHouse
                    });
                }
            }

            await _context.SaveChangesAsync();
            StatusMessage = "Vừa đăng tin, vui trong chờ quản trị viên phê duyệt";
            return RedirectToAction(nameof(Index));
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

            // Tăng lượt xem lên 1 đơn vị
            roomingHouse.ViewCount++;
            _context.Update(roomingHouse);
            await _context.SaveChangesAsync();

            var result = new RoomingHouseViewModel()
            {
                RoomingHouse = roomingHouse
            };
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var post = await _context.Posts.FindAsync(id);
            var roomingHouse = await _context.RoomingHouses.Include(p => p.RoomingHouseAreas)
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
                Image = roomingHouse.Image,
                Description = roomingHouse.Description,
                Published = false,
                AreaIDs = roomingHouse.RoomingHouseAreas.Select(pc => pc.AreaId).ToArray()
            };

            var areas = await _context.Areas.ToListAsync();
            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            return View(roomingHouseEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image,Content,Price,AreaIDs")] CreateRoomingHouseModel roomingHouse)
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

                    var roomingHousesUpdate = await _context.RoomingHouses.Include(p => p.RoomingHouseAreas).FirstOrDefaultAsync(p => p.Id == id);
                    if (roomingHousesUpdate == null)
                    {
                        return NotFound();
                    }

                    roomingHousesUpdate.Title = roomingHouse.Title;
                    roomingHousesUpdate.Content = roomingHouse.Content;
                    roomingHousesUpdate.DateUpdated = DateTime.Now;
                    roomingHousesUpdate.Price = roomingHouse.Price;
                    roomingHousesUpdate.Description = roomingHouse.Description;
                    roomingHousesUpdate.Image = roomingHouse.Image;

                    roomingHouse.AreaIDs ??= new int[] { };
                    var oldAreaIds = roomingHousesUpdate.RoomingHouseAreas.Select(c => c.AreaId).ToArray();
                    var newAreaIds = roomingHouse.AreaIDs;

                    var removeRoomingHousesAreas = from roomingHousesAreas in roomingHousesUpdate.RoomingHouseAreas
                                          where (!newAreaIds.Contains(roomingHousesAreas.AreaId))
                                          select roomingHousesAreas;
                    _context.RoomingHouseAreas.RemoveRange(removeRoomingHousesAreas);

                    var addRoomingHousesAreas = from areaId in newAreaIds
                                     where !oldAreaIds.Contains(areaId)
                                     select areaId;

                    foreach (var areasId in addRoomingHousesAreas)
                    {
                        _context.RoomingHouseAreas.Add(new RoomingHouseArea()
                        {
                            RoomingHouseId = id,
                            AreaId = areasId
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
                StatusMessage = "Vừa cập nhật bản tin, vui lòng chờ quản trị viên xét duyệt";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", roomingHouse.AuthorId);
            return View(roomingHouse);
        }

        private bool RoomingHouseExists(int id)
        {
            return _context.RoomingHouses.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomingHouse = await _context.RoomingHouses
                .Include(p => p.Comments)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomingHouse == null)
            {
                return NotFound();
            }

            return View(roomingHouse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomingHouse = await _context.RoomingHouses
                .Include(p => p.Comments)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (roomingHouse == null)
            {
                return NotFound();
            }
            // Xóa comment liên quan
            _context.Comments.RemoveRange(roomingHouse.Comments);
            _context.RoomingHouses.Remove(roomingHouse);
            await _context.SaveChangesAsync();

            StatusMessage = "Bạn vừa xóa bài đăng " + roomingHouse.Title;

            return RedirectToAction(nameof(Index));
        }

    }
}
