using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyBlog.Areas.Identity.Models;

public class BlogUser : IdentityUser
{
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }

    [DataType(DataType.Date)]
    public DateTime? CreatedAt { get; set; } // ngay tao tai khoan
}