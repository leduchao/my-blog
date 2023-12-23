using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog.Services;

public class PostsService : IPostsService
{
    private readonly MyBlogDbContext _context;
    private readonly IRepository<Post> _postsRepository;

    private readonly int _pageSize;

    public PostsService(MyBlogDbContext context, IRepository<Post> postsRepository)
    {
        _context = context;
        _pageSize = 12;
        _postsRepository = postsRepository;
    }

    // tạo bài viết mới
    public async Task<bool> CreatePostAsync(PostDTO request)
    {
        try
        {
            var newPost = new Post
            {
                Title = request.Title.ToUpper(),
                Content = request.Content,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                CreatedBy = "Admin",
                Tags = request.Tags
            };

            if (request.Image is not null)
            {
                newPost.Image = SetFileName(request.Image);
                await UploadImageAsync(request.Image);
            }

            _postsRepository.Add(newPost);
            await _postsRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        return true;
    }

    // xóa post theo id
    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _postsRepository.GetByIdAsync(id);

        if (post is null)
            return false;

        File.Delete(@"wwwroot\images\" + post.Image);
        _postsRepository.Delete(post);

        await _postsRepository.SaveChangesAsync();

        return true;
    }

    // tìm bài viết theo id
    public async Task<Post?> FindPostByIdAsync(int id) => await _postsRepository.GetByIdAsync(id);

    // hiển thị danh sách bài viết
    public async Task<List<Post>?> GetAllPostsAsync()
    {
        return await _postsRepository.GetAllAsync();
    }

    // cập nhật bài viết
    public async Task<bool> UpdatePostAsync(int id, PostDTO request)
    {
        var postUpdate = await _postsRepository.GetByIdAsync(id);

        if (postUpdate is null)
            return false;

        string oldImage = postUpdate.Image; // old image path

        if (request.Image is not null)
        {
            File.Delete(@"wwwroot\images\" + oldImage); // delete old image

            // set name and upload new image
            postUpdate.Image = SetFileName(request.Image);
            await UploadImageAsync(request.Image);
        }

        postUpdate.Title = request.Title.ToUpper();
        postUpdate.Content = request.Content;
        postUpdate.LastUpdatedAt = DateTime.Now;
        postUpdate.Tags = request.Tags;

        _postsRepository.Update(postUpdate);
        await _postsRepository.SaveChangesAsync();

        return true;
    }

    public async Task UploadImageAsync(IFormFile file)
    {
        if (file is not null)
        {
            var filePath = Path.Combine("wwwroot", "images", SetFileName(file));

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }
    }

    public string SetFileName(IFormFile file) =>
        DateTime.Now.ToString("yyyyMMddhhmmsstt") + "_" + Path.GetFileName(file.FileName);

    public async Task<List<Post>> SearchPostsAsync(string keyword)
    {
        var listSearchedPosts = await _postsRepository
            .GetQueryable()
            .Where(p =>
                p.Title.ToLower().Contains(keyword.ToLower()) ||
                p.Tags.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();

        return listSearchedPosts;
    }
}