using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateExtensions;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.Models.AggregateUser;

namespace ProjectFinalEngineer.Controllers
{
    [Authorize(Roles = RoleName.Administrator)]
    [Route("/ManageUser/[action]")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public UserController(
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage)
        {
            var model = new UserListModel
            {
                CurrentPage = currentPage
            };

            var qr = _userManager.Users.OrderBy(u => u.UserName);

            model.TotalUsers = await qr.CountAsync();
            model.CountPages = (int)Math.Ceiling((double)model.TotalUsers / model.ItemsPerPage);

            if (model.CurrentPage < 1)
                model.CurrentPage = 1;
            if (model.CurrentPage > model.CountPages)
                model.CurrentPage = model.CountPages;

            var qr1 = qr.Skip((model.CurrentPage - 1) * model.ItemsPerPage)
                        .Take(model.ItemsPerPage)
                        .Select(u => new UserAndRole()
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                        });

            model.Users = await qr1.ToListAsync();

            foreach (var user in model.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(",", roles);
            }

            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> AddRoleAsync(string id)
        {
            var model = new AddUserRoleModel();
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không tìm thấy giá trị thỏa mãn");
            }

            model.User = await _userManager.FindByIdAsync(id);

            if (model.User == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            model.RoleNames = (await _userManager.GetRolesAsync(model.User)).ToArray();

            var roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);

            await GetClaims(model);

            return View(model);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleAsync(string id, [Bind("RoleNames")] AddUserRoleModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không tìm thấy giá trị thỏa mãn");
            }

            model.User = await _userManager.FindByIdAsync(id);

            if (model.User == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }
            await GetClaims(model);

            var oldRoleNames = (await _userManager.GetRolesAsync(model.User)).ToArray();

            var deleteRoles = oldRoleNames.Where(r => !model.RoleNames.Contains(r));
            var addRoles = model.RoleNames.Where(r => !oldRoleNames.Contains(r));

            var roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            ViewBag.allRoles = new SelectList(roleNames);

            var resultDelete = await _userManager.RemoveFromRolesAsync(model.User, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                ModelState.AddModelError(resultDelete);
                return View(model);
            }

            var resultAdd = await _userManager.AddToRolesAsync(model.User, addRoles);
            if (!resultAdd.Succeeded)
            {
                ModelState.AddModelError(resultAdd);
                return View(model);
            }

            StatusMessage = $"Vừa cập nhật vai trò cho thành viên: {model.User.UserName}";

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetPasswordAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            return View();
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPasswordAsync(string id, SetUserPasswordModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userManager.RemovePasswordAsync(user);

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            StatusMessage = $"Vừa cập nhật mật khẩu cho tài khoản: {user.UserName}";

            return RedirectToAction("Index");
        }


        [HttpGet("{userid}")]
        public async Task<ActionResult> AddClaimAsync(string userid)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            ViewBag.user = user;
            return View();
        }

        [HttpPost("{userid}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddClaimAsync(string userid, AddUserClaimModel model)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy thành viên");
            ViewBag.user = user;
            if (!ModelState.IsValid) return View(model);
            var claims = _context.UserClaims.Where(c => c.UserId == user.Id);

            if (claims.Any(c => c.ClaimType == model.ClaimType && c.ClaimValue == model.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Quyền này đã có");
                return View(model);
            }

            await _userManager.AddClaimAsync(user, new Claim(model.ClaimType, model.ClaimValue));
            StatusMessage = "Đã thêm quyền cho thành viên";

            return RedirectToAction("AddRole", new { id = user.Id });
        }

        [HttpGet("{claimId:int}")]
        public async Task<IActionResult> EditClaim(int claimId)
        {
            var userClaim = _context.UserClaims.FirstOrDefault(c => c.Id == claimId);
            var user = await _userManager.FindByIdAsync(userClaim?.UserId);

            if (user == null) return NotFound("Không tìm thấy thành viên");

            var model = new AddUserClaimModel();

            if (userClaim == null) return View("AddClaim", model);
            model.ClaimType = userClaim.ClaimType;
            model.ClaimValue = userClaim.ClaimValue;
            ViewBag.user = user;
            ViewBag.userclaim = userClaim;

            return View("AddClaim", model);
        }

        [HttpPost("{claimId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClaim(int claimId, AddUserClaimModel model)
        {
            var userClaim = _context.UserClaims.FirstOrDefault(c => c.Id == claimId);
            var user = await _userManager.FindByIdAsync(userClaim?.UserId);
            if (user == null) return NotFound("Không tìm thấy user");

            if (!ModelState.IsValid) return View("AddClaim", model);

            if (_context.UserClaims.Any(c => c.UserId == user.Id
                && c.ClaimType == model.ClaimType
                && c.ClaimValue == model.ClaimValue
                && c.Id != userClaim.Id))
            {
                ModelState.AddModelError("Quyền này đã có");
                return View("AddClaim", model);
            }

            if (userClaim != null)
            {
                userClaim.ClaimType = model.ClaimType;
                userClaim.ClaimValue = model.ClaimValue;
            }

            await _context.SaveChangesAsync();
            StatusMessage = "Bạn vừa cập nhật quyền";

            ViewBag.user = user;
            ViewBag.userclaim = userClaim;
            return RedirectToAction("AddRole", new { id = user.Id });
        }

        [HttpPost("{claimId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClaimAsync(int claimId)
        {
            var userClaim = _context.UserClaims.FirstOrDefault(c => c.Id == claimId);
            var user = await _userManager.FindByIdAsync(userClaim?.UserId);

            if (user == null) return NotFound("Không tìm thấy thành viên");

            if (userClaim != null)
                await _userManager.RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));

            StatusMessage = "Bạn đã xóa quyền";

            return RedirectToAction("AddRole", new { id = user.Id });
        }

        private async Task GetClaims(AddUserRoleModel model)
        {
            var listRoles = from r in _context.Roles
                            join ur in _context.UserRoles on r.Id equals ur.RoleId
                            where ur.UserId == model.User.Id
                            select r;

            var claimsInRole = from c in _context.RoleClaims
                                join r in listRoles on c.RoleId equals r.Id
                                select c;
            model.ClaimsInRole = await claimsInRole.ToListAsync();

            model.ClaimsInUserClaim = await (from c in _context.UserClaims
                                             where c.UserId == model.User.Id
                                             select c).ToListAsync();
        }
    }
}
