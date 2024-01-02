using MyBlog.Models;

namespace MyBlog.Services;

public interface IPostsService
{
    Task<List<Post>?> GetAllPostsAsync();

    Task<Post?> FindPostByIdAsync(int id);

    Task<bool> CreatePostAsync(PostDTO request);

    Task<bool> UpdatePostAsync(int id, PostDTO request);

    Task<bool> DeletePostAsync(int id);

    Task<List<Post>> SearchPostsAsync(string keyword);

    string SetFileName(IFormFile file);

    Task UploadImageAsync(IFormFile file);
}