using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace MyBlog.Areas.Identity.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Sao không nhập email vậy bồ?")]
    [EmailAddress(ErrorMessage = "{0} không đúng định dạng!")]
    [StringLength(100, ErrorMessage = "{0} có dộ dài từ {2} đến {1} ký tự!", MinimumLength = 5)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Làm sao có thể đăng nhập khi không có pass?")]
    [StringLength(20, ErrorMessage = "Mật khẩu có dộ dài từ {2} đến {1} ký tự!", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool IsRememberMe { get; set; }

    public IEnumerable<AuthenticationScheme>? ExternalAccounts { get; set; }
}