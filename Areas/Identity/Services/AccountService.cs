using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MyBlog.Areas.Identity.Models;

namespace MyBlog.Areas.Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<BlogUser> _userManager;

    private readonly SignInManager<BlogUser> _signInManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IConfiguration _configuration;

    public AccountService(
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<bool> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email!);

        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(
                user: user,
                password: model.Password!,
                isPersistent: model.IsRememberMe,
                lockoutOnFailure: true);

            return result.Succeeded;
        }

        return false;
    }

    public async Task<bool> Register(RegisterModel model)
    {
        var newUser = new BlogUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            Email = model.Email,
            CreatedAt = DateTime.Now,
        };

        var result = await _userManager.CreateAsync(newUser, model.Password!);

        if (result.Succeeded)
        {
            await AddRoleAsync(newUser);
            return true;
        }

        return false;
    }

    public async Task<bool> SendVerifyEmaiCodelAsync(string link, string email)
    {
        var result = await SendCodeAsync(link, email);
        return result;
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return false;

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return false;

        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded;
    }

    public async Task DeleteUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        await _userManager.DeleteAsync(user!);
    }

    public async Task<BlogUser?> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }

    public async Task<string> GenerateToken(BlogUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        return token;
    }

    public Task<bool> ChangePassword()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetPassword()
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        return _signInManager.SignOutAsync();
    }

    private async Task<bool> SendCodeAsync(string link, string mailTo)
    {
        var mailSettingsSections = _configuration.GetSection("MailSettings");

        var mailSettings = new MailSettings(
            mailSettingsSections["MailAddress"]!,
            mailSettingsSections["Password"]!,
            mailSettingsSections["Host"]!,
            Convert.ToInt32(mailSettingsSections["Port"]));

        string subject = "Xác thực email";
        string body = $"Click vào link sau để xác thực: <a href='{link}'>{link}</a>";

        using var client = new SmtpClient(mailSettings.Host)
        {
            Port = Convert.ToInt32(mailSettings.Port),

            Credentials = new NetworkCredential(
                userName: mailSettings.MailFrom,
                password: mailSettings.Password),

            EnableSsl = true,
        };

        try
        {
            var message = new MailMessage(
                from: mailSettings.MailFrom!,
                to: mailTo,
                subject: subject,
                body: body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    private async Task AddRoleAsync(BlogUser user)
    {
        if (await _roleManager.RoleExistsAsync("Admin") == false)
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            var usersWithAdminRole = await _userManager.GetUsersInRoleAsync("Admin");

            if (usersWithAdminRole.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                if (!await _roleManager.RoleExistsAsync("Viewer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Viewer"));
                    await _userManager.AddToRoleAsync(user, "Viewer");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Viewer");
                }
            }
        }
    }
}