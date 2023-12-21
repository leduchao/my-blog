using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using MyBlog.Services;

namespace MyBlog;

[Route("posts")]
public class PostsController : Controller
{
    private readonly IPostsService _service;

    public PostsController(IPostsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        page ??= 1;
        ViewBag.TotalPages = _service.NumberOfPages();
        ViewBag.NextPage = page + 1;
        ViewBag.PreviousPage = (page == 0 || page == 1) ? 1 : page - 1;

        return View(await _service.ShowPostsAsync((int)page));
    }

    [Route("create-post")]
    [Authorize(Roles = "Admin")]
    public IActionResult CreatePost()
    {
        return View();
    }

    [HttpPost]
    [Route("create-post")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePost(PostDTO request)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.CreatePostAsync(request);
            return RedirectToAction(nameof(Index), "Posts");
        }

        return View(request);
    }

    [Route("show-post-detail")]
    [AllowAnonymous]
    public async Task<IActionResult> ShowPostDetail(int id)
    {
        var post = await _service.FindPostByIdAsync(id);

        if (post == null)
            return NotFound();

        return View(post);
    }

    [Route("edit-post")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditPost(int id)
    {
        var post = await _service.FindPostByIdAsync(id);

        if (post == null)
            return NotFound();

        var postDto = new PostDTO
        {
            Id = id,
            Title = post.Title,
            Content = post.Content,
            Tags = post.Tags
        };

        return View(postDto);
    }

    [HttpPost]
    [Route("edit-post")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditPost(int id, PostDTO request)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdatePostAsync(id, request);

            if (result)
                return RedirectToAction(nameof(ShowPostDetail), new { id });
        }

        request.Id = id;
        return RedirectToAction(nameof(EditPost), new { id });
        //return View(request);
    }

    [Route("delete-post")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await _service.DeletePostAsync(id);

        if (result)
            return RedirectToAction(nameof(Index), "Posts");

        return RedirectToAction(nameof(ShowPostDetail), new { id });
    }

    [HttpGet]
    [Route("search-posts")]
    public async Task<IActionResult> SearchPosts(string keyword)
    {
        if (!string.IsNullOrEmpty(keyword))
        {
            ViewBag.Keyword = keyword;
            return View(await _service.SearchPostsAsync(keyword));
        }

        return RedirectToAction(nameof(Index));
    }
}
