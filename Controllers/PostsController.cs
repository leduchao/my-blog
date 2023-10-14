using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using MyBlog.Services;

namespace MyBlog
{
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

            return View(await _service.ShowPosts((int)page));
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
                var result = await _service.CreatePost(request);
                return RedirectToAction(nameof(Index), "Posts");
            }

            return View(request);
        }

        [Route("show-post-detail")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowPostDetail(int id)
        {
            var post = await _service.FindPostById(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [Route("edit-post")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _service.FindPostById(id);

            if (post == null)
                return NotFound();

            ViewBag.PostId = post.Id;
            return View(new PostDTO { Title = post.Title, Content = post.Content });
        }

        [HttpPost]
        [Route("edit-post")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int id, PostDTO request)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdatePost(id, request);

                if (result)
                    return RedirectToAction(nameof(ShowPostDetail), new { id });
            }

            return RedirectToAction(nameof(EditPost), new { id });
        }

        [Route("delete-post")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _service.DeletePost(id);

            if (result)
                return RedirectToAction(nameof(Index), "Posts");

            return RedirectToAction(nameof(ShowPostDetail), new { id });
        }
    }
}
