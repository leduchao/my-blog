using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải nhập tên danh mục!")]
        [StringLength(50, ErrorMessage = "{0} có độ dài ít nhất là {2} và dài nhất là {1}!", MinimumLength = 1)]
        [DisplayName("Tên danh mục")]
        public string Name { get; set; } = string.Empty;
    }
}