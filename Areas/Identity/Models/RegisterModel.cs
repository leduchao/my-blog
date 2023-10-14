using System.ComponentModel.DataAnnotations;

namespace MyBlog.Areas.Identity.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Hãy chọn một cho bạn 1 cái tên!")]
    [StringLength(
        100,
        ErrorMessage = "Tên người dùng có dộ dài từ {2} đến {1} ký tự!",
        MinimumLength = 5)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Hãy điền email của bạn!")]
    [EmailAddress(ErrorMessage = "{0} không đúng định dạng!")]
    [StringLength(
        100,
        ErrorMessage = "{0} có độ dài từ {2} đến {1} ký tự!",
        MinimumLength = 5)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Không được bỏ trống mật khẩu!")]
    [DataType(DataType.Password)]
    [StringLength(
        20,
        ErrorMessage = "Mật khẩu phải có độ dài từ {2} đến {1} ký tự",
        MinimumLength = 6)]
    [Display(Name = "Mật khẩu")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Hãy điền lại mật khẩu để xác minh!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp!")]
    public string? ConfirmPassword { get; set; }

    public bool IsAgree { get; set; }
}