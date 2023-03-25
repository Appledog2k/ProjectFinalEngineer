using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.EntityFramework;
using ProjectFinalEngineer.Models.AggregateExtensions;
using ProjectFinalEngineer.Models.AggregateRole;
using ProjectFinalEngineer.Models.AggregateUser;

namespace ProjectFinalEngineer.Controllers
{

    [Authorize(Roles = RoleName.Administrator)]
    [Route("/Role/[action]")]
    public class RoleController : Controller
    {

        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public RoleController(ILogger<RoleController> logger, RoleManager<IdentityRole> roleManager, AppDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var r = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var roles = new List<RoleModel>();
            foreach (var _r in r)
            {
                var claims = await _roleManager.GetClaimsAsync(_r);
                var claimsString = claims.Select(c => c.Type + "=" + c.Value);

                var rm = new RoleModel()
                {
                    Name = _r.Name,
                    Id = _r.Id,
                    Claims = claimsString.ToArray()
                };
                roles.Add(rm);
            }

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var newRole = new IdentityRole(model.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo vai trò mới mới: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }
            return View();
        }

        [HttpGet("{roleid}")]
        public async Task<IActionResult> DeleteAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy vai trò");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }
            return View(role);
        }

        [HttpPost("{roleid}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy vai trò");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("Không tìm thấy vai trò");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa: {role.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }
            return View(role);
        }

        [HttpGet("{roleid}")]
        public async Task<IActionResult> EditAsync(string roleid, [Bind("Name")] EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy vai trò");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }
            model.Name = role.Name;
            model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            model.Role = role;
            ModelState.Clear();
            return View(model);

        }

        [HttpPost("{roleid}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmAsync(string roleid, [Bind("Name")] EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy vai trò");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }
            model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            model.Role = role;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }

            return View(model);
        }

        [HttpGet("{roleid}")]
        public async Task<IActionResult> AddRoleClaimAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy vai trò");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }

            var model = new EditClaimModel()
            {
                Role = role
            };
            return View(model);
        }

        [HttpPost("{roleid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleClaimAsync(string roleid, [Bind("ClaimType", "ClaimValue")] EditClaimModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy vai trò");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }
            model.Role = role;
            if (!ModelState.IsValid) return View(model);


            if ((await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == model.ClaimType && c.Value == model.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong vai trò");
                return View(model);
            }

            var newClaim = new Claim(model.ClaimType, model.ClaimValue);
            var result = await _roleManager.AddClaimAsync(role, newClaim);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(result);
                return View(model);
            }

            StatusMessage = "Vừa thêm đặc tính (claim) mới";

            return RedirectToAction("Edit", new { roleid = role.Id });

        }

        [HttpGet("{claimid:int}")]
        public async Task<IActionResult> EditRoleClaim(int claimid)
        {
            var claim = _context.RoleClaims.FirstOrDefault(c => c.Id == claimid);
            if (claim == null) return NotFound("Không tìm thấy vai trò");

            var role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null) return NotFound("Không tìm thấy vai trò");
            ViewBag.claimid = claimid;

            var Input = new EditClaimModel()
            {
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
                Role = role
            };


            return View(Input);
        }
        [HttpPost("{claimid:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoleClaim(int claimid, [Bind("ClaimType", "ClaimValue")] EditClaimModel Input)
        {
            var claim = _context.RoleClaims.FirstOrDefault(c => c.Id == claimid);
            if (claim == null) return NotFound("Không tìm thấy vai trò");

            ViewBag.claimid = claimid;

            var role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null) return NotFound("Không tìm thấy vai trò");
            Input.Role = role;
            if (!ModelState.IsValid)
            {
                return View(Input);
            }
            if (_context.RoleClaims.Any(c => c.RoleId == role.Id && c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue && c.Id != claim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong vai trò");
                return View(Input);
            }


            claim.ClaimType = Input.ClaimType;
            claim.ClaimValue = Input.ClaimValue;

            await _context.SaveChangesAsync();

            StatusMessage = "Vừa cập nhật claim";

            return RedirectToAction("Edit", new { roleid = role.Id });
        }
  
        [HttpPost("{claimid:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClaim(int claimid, [Bind("ClaimType", "ClaimValue")] EditClaimModel Input)
        {
            var claim = _context.RoleClaims.FirstOrDefault(c => c.Id == claimid);
            if (claim == null) return NotFound("Không tìm thấy vai trò");

            var role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null) return NotFound("Không tìm thấy vai trò");
            Input.Role = role;
            if (!ModelState.IsValid)
            {
                return View(Input);
            }
            if (_context.RoleClaims.Any(c => c.RoleId == role.Id && c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue && c.Id != claim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong vai trò");
                return View(Input);
            }


            await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));

            StatusMessage = "Vừa xóa claim";


            return RedirectToAction("Edit", new { roleid = role.Id });
        }


    }
}
