using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models;

public class PostDTO
{
    [Required(ErrorMessage = "Id không phù hơp!")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Phải nhập tiêu đề bài viết!")]
    [StringLength(100, ErrorMessage = "{0} có độ dài ít nhất là {2} và dài nhất là {1}!", MinimumLength = 3)]
    [Display(Name = "Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Hãy chọn hình ảnh cho bài post!")]
    [Display(Name = "Hình ảnh minh họa")]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Bài viết cần có nội dung!")]
    [MinLength(10, ErrorMessage = "Bài viết phải có ít nhất 1 câu!")]
    [Display(Name = "Nội dung bài viết")]
    public string Content { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Hãy nhập ít nhất 1 hashtag!")]
    [Display(Name = "Hashtags: ")]
    public string Tags { get; set; } = string.Empty;
}