using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.Models.RoomingHouse;

namespace ProjectFinalEngineer.Controllers
{

    [Route("admin/forum/area/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AreaController : Controller
    {
        private readonly AppDbContext _context;

        public AreaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Forum/Category
        public async Task<IActionResult> Index()
        {
            var qr = (from c in _context.Areas select c)
                .Include(c => c.ParentArea)
                .Include(c => c.AreaChildren);

            var areas = (await qr.ToListAsync())
                .Where(c => c.ParentArea == null)
                .ToList();

            return View(areas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .Include(c => c.ParentArea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        private void CreateSelectItems(List<Area> source, List<Area> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var area in source)
            {
                des.Add(new Area()
                {
                    Id = area.Id,
                    Title = prefix + " " + area.Title
                });
                if (area.AreaChildren?.Count > 0)
                {
                    CreateSelectItems(area.AreaChildren.ToList(), des, level + 1);
                }
            }
        }

        public async Task<IActionResult> CreateAsync()
        {
            var qr = (from c in _context.Areas select c)
                .Include(c => c.ParentArea)
                .Include(c => c.AreaChildren);

            var areas = (await qr.ToListAsync())
                .Where(c => c.ParentArea == null)
                .ToList();
            areas.Insert(0, new Area()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Area>();
            CreateSelectItems(areas, items, 0);
            var selectList = new SelectList(items, "Id", "Title");


            ViewData["ParentAreaId"] = selectList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ParentAreaId")] Area area)
        {
            if (ModelState.IsValid)
            {
                if (area.ParentAreaId == -1) area.ParentAreaId = null;
                _context.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            var qr = (from c in _context.Areas select c)
                .Include(c => c.ParentArea)
                .Include(c => c.AreaChildren);

            var areas = (await qr.ToListAsync())
                .Where(c => c.ParentArea == null)
                .ToList();
            areas.Insert(0, new Area()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Area>();
            CreateSelectItems(areas, items, 0);
            var selectList = new SelectList(items, "Id", "Title");


            ViewData["ParentCategoryId"] = selectList;
            return View(area);
        }

        // GET: Blog/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            var qr = (from c in _context.Areas select c)
                     .Include(c => c.ParentArea)
                     .Include(c => c.AreaChildren);

            var areas = (await qr.ToListAsync())
                             .Where(c => c.ParentArea == null)
                             .ToList();
            areas.Insert(0, new Area()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Area>();
            CreateSelectItems(areas, items, 0);
            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentAreaId"] = selectList;
            return View(area);
        }


        // POST: Blog/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ParentAreaId")] Area area)
        {
            if (id != area.Id)
            {
                return NotFound();
            }

            bool canUpdate = true;

            if (area.ParentAreaId == area.Id)
            {
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
                canUpdate = false;
            }

            // Kiem tra thiet lap muc cha phu hop
            if (canUpdate && area.ParentAreaId != null)
            {
                var childCates =
                            (from c in _context.Areas select c)
                            .Include(c => c.AreaChildren)
                            .ToList()
                            .Where(c => c.ParentAreaId == area.Id);


                // Func check Id 
                Func<List<Area>, bool> checkCateIds = null;
                checkCateIds = (cates) =>
                {
                    foreach (var cate in cates)
                    {
                        Console.WriteLine(cate.Title);
                        if (cate.Id == area.ParentAreaId)
                        {
                            canUpdate = false;
                            ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khácXX");
                            return true;
                        }
                        if (cate.AreaChildren != null)
                            return checkCateIds(cate.AreaChildren.ToList());

                    }
                    return false;
                };
                // End Func 
                checkCateIds(childCates.ToList());
            }

            if (ModelState.IsValid && canUpdate)
            {
                try
                {
                    if (area.ParentAreaId == -1)
                        area.ParentAreaId = null;

                    var dtc = _context.Areas.FirstOrDefault(c => c.Id == id);
                    _context.Entry(dtc).State = EntityState.Detached;

                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var qr = (from c in _context.Areas select c)
                     .Include(c => c.ParentArea)
                     .Include(c => c.AreaChildren);

            var categories = (await qr.ToListAsync())
                             .Where(c => c.ParentArea == null)
                             .ToList();

            categories.Insert(0, new Area()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Area>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;


            return View(area);
        }

        private bool AreaExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        // GET: Blog/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Areas
                .Include(c => c.ParentArea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Blog/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Areas
                .Include(c => c.AreaChildren)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            foreach (var cCategory in category.AreaChildren)
            {
                cCategory.ParentAreaId = category.ParentAreaId;
            }


            _context.Areas.Remove(category);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

    }
}
