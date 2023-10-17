using MyBlog.Models;

namespace MyBlog.Services;

public interface IPostsService
{
    Task<List<Post>?> ShowPostsAsync(int currentPage);

    //int GetAllPosts();

    int NumberOfPages();

    Task<Post?> FindPostByIdAsync(int id);

    Task<bool> CreatePostAsync(PostDTO request);//, string tags);

    Task<bool> UpdatePostAsync(int id, PostDTO request);

    Task<bool> DeletePostAsync(int id);
}