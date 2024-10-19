using Microsoft.AspNetCore.Mvc;

namespace ControllerRouting.Controllers;

public class BlogController : Controller
{
    public IActionResult Index(string path)
    {
        return Json($"Blog at path '{path}'");
    }
}
