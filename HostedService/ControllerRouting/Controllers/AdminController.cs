using Microsoft.AspNetCore.Mvc;

namespace MvcRouting.Controllers;

[Area("admin")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return Json("Admin content");
    }
}
