using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models;

public class PostDTO
{
    [Required(ErrorMessage = "Phải nhập tiêu đề bài viết!")]
    [StringLength(100, ErrorMessage = "{0} có độ dài ít nhất là {2} và dài nhất là {1}!", MinimumLength = 3)]
    [DisplayName("Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hãy chọn hình ảnh cho bài post!")]
    [DisplayName("Hình ảnh minh họa")]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Bài viết cần có nội dung!")]
    [MinLength(10, ErrorMessage = "Bài viết phải có ít nhất 1 câu!")]
    [DisplayName("Nội dung bài viết")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hãy nhập ít nhất 1 hashtag!")]
    [DisplayName("Hashtags: ")]
    public string Tags { get; set; } = string.Empty;
}