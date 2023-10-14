using MyBlog.Areas.Identity.Models;

namespace MyBlog.Areas.Identity.Services;

public interface IAccountService
{
    Task<bool> Register(RegisterModel model);

    Task<bool> Login(LoginModel model);

    Task Logout();

    Task<bool> ResetPassword();

    Task<bool> ChangePassword();

    Task<bool> SendVerifyEmaiCodelAsync(string link, string email);

    Task<bool> ConfirmEmailAsync(string userId, string code);

    Task DeleteUser(string email);

    Task<BlogUser?> FindByEmailAsync(string email);

    Task<string> GenerateToken(BlogUser user);
}