using Microsoft.AspNetCore.Mvc;

namespace MvcRouting.Controllers;

[Route("api/blogs")]
[ApiController]
public class BlogApiController : ControllerBase
{
    [HttpGet("{*path}")]
    public ActionResult<string> GetBlog(string? path)
    {
        return $"Blog at path '{path}'";
    }
}
