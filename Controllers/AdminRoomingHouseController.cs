using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateExtensions;
using ProjectFinalEngineer.Models.RoomingHouse;


namespace ProjectFinalEngineer.Controllers
{
    [Route("/forum/AdminRoomingHouse/[action]/{id:int?}")]
    public class AdminRoomingHouseController : Controller
    {
        private readonly AppDbContext _context;
        public AdminRoomingHouseController(AppDbContext context)
        {
            this._context = context;
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
                .Include(p => p.Comments)
                .Where(x => x.Published == false);

            if (searchString != null)
            {
                roomingHouses = roomingHouses.Where(post => post.Title.ToLower().Contains(searchString.ToLower()) || post.Content.Contains(searchString.ToLower()));
            }

            int totalPosts = await roomingHouses.CountAsync();
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

            var roomingHousesInPage = await roomingHouses.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .Include(p => p.RoomingHouseAreas)
                .ThenInclude(pc => pc.Area)
                .ToListAsync();

            return View(roomingHousesInPage);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveRoomingHouse(int id)
        {
            var roomingHouse = _context.RoomingHouses.Find(id);
            if (roomingHouse != null)
            {
                roomingHouse.Published = true;
                roomingHouse.DateUpdated = DateTime.Now;
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
                Image = roomingHouse.Image,
                Price = roomingHouse.Price,
                Published = false,
                AreaIDs = roomingHouse.RoomingHouseAreas.Select(pc => pc.AreaId).ToArray()
            };

            var areas = await _context.Areas.ToListAsync();
            ViewData["areas"] = new MultiSelectList(areas, "Id", "Title");

            return View(roomingHouseEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refuse(int id, [Bind("Id,Title,Reason,Image")] CreateRoomingHouseModel roomingHouse)
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
                    roomingHousesUpdate.Image = roomingHouse.Image;
                    roomingHousesUpdate.DateUpdated = DateTime.Now;
                    roomingHousesUpdate.Reason = roomingHouse.Reason;
                    roomingHousesUpdate.Priority = 2;
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
    }
}
