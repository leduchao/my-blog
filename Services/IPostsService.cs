using MyBlog.Models;

namespace MyBlog.Services;

public interface IPostsService
{
    Task<List<Post>?> ShowPostsAsync(int currentPage);

    int NumberOfPages();

    Task<Post?> FindPostByIdAsync(int id);

    Task<bool> CreatePostAsync(PostDTO request);

    Task<bool> UpdatePostAsync(int id, PostDTO request);

    Task<bool> DeletePostAsync(int id);

    Task<List<Post>> SearchPostsAsync(string keyword);
}