using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectFinalEngineer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleName.Administrator)]
    public class Admin : Controller
    {
        [Route("/admin/")]
        public IActionResult Index() => View();
    }
}