using MyBlog.Models;

namespace MyBlog.Services;

public interface IPostsService
{
    Task<List<Post>?> ShowPosts(int currentPage);

    //int GetAllPosts();

    int NumberOfPages();

    Task<Post?> FindPostById(int id);

    Task<bool> CreatePost(PostDTO request);

    Task<bool> UpdatePost(int id, PostDTO request);

    Task<bool> DeletePost(int id);
}