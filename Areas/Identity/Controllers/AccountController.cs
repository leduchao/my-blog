using Microsoft.AspNetCore.Mvc;
using MyBlog.Areas.Identity.Models;
using MyBlog.Areas.Identity.Services;

namespace MyBlog.Areas.Identity.Controllers;

[Area("Identity")]
[Route("identity/account")]
public class AccountController : Controller
{
    private readonly IAccountService _service;

    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountService service,
        ILogger<AccountController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Route("login")]
    public async Task<IActionResult> Login()
    {
        await _service.Logout();
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([Bind("Email, Password")] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.Login(model);

            if (result)
            {
                _logger.LogInformation("User logged in");
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Email hoặc mật khẩu không đúng!");

                return View(model);
            }
        }
        return View();
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.Register(model);
            _logger.LogInformation("User account has been created!");

            var user = await _service.FindByEmailAsync(model.Email!);

            if (result == true && user != null)
            {
                string code = await _service.GenerateToken(user);

                var callbackUrl = Url.ActionLink(
                    action: "ConfirmEmail",
                    controller: "Account",
                    values: new { area = "Identity", userId = user.Id, token = code },
                    protocol: Request.Scheme);

                if (callbackUrl != null)
                {
                    var isSended = await _service.SendVerifyEmaiCodelAsync(
                        callbackUrl,
                        model.Email!);

                    if (isSended)
                        return RedirectToAction(nameof(ConfirmEmail));

                    _logger.LogError("Can not send verify link to your email!");
                    await _service.DeleteUser(model.Email!);

                    return View(model);
                }
            }
            else
            {
                _logger.LogError("Can not register new user!");
                await _service.DeleteUser(model.Email!);

                return View(model);
            }
        }

        ModelState.AddModelError(
            string.Empty,
            "Thông tin không hợp lệ, vui lòng kiểm tra lại!");

        _logger.LogError("Something went wrong!");

        return View(model);
    }

    [Route("confirm-email")]
    public IActionResult ConfirmEmail()
    {
        return View();
    }

    [HttpGet]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var result = await _service.ConfirmEmailAsync(userId, token);

        if (result)
        {
            ViewBag.IsSuccess = true;
            return RedirectToAction(nameof(Login));
        }

        return View();
    }

    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _service.Logout();
        _logger.LogInformation("User logged out!");

        return RedirectToAction("Index", "Home", new { area = "" });
    }

    [Route("access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }
}