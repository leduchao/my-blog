using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers;

[Route("discussion")]
public class DiscussionsController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}