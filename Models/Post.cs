using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public int Views { get; set; } = 0;

        public int Likes { get; set; } = 0;

        public List<Tag>? Tags { get; set; }
    }
}