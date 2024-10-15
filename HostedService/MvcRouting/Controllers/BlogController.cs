using Microsoft.AspNetCore.Mvc;

namespace MvcRouting.Controllers;

public class BlogController : Controller
{
    public IActionResult Index(string path)
    {
        return Json($"Blog at path '{path}'");
    }
}
