using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog.Services;

public class PostsService : IPostsService
{
    private readonly MyBlogDbContext _context;

    private readonly int _pageSize;

    public PostsService(MyBlogDbContext context)
    {
        _context = context;
        _pageSize = 12;
    }

    // tạo bài viết mới
    public async Task<bool> CreatePostAsync(PostDTO request)//, string tags)
    {
        var newPost = new Post
        {
            Title = request.Title.ToUpper(),
            Image = SetFileName(request.Image!),
            Content = request.Content,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
            CreatedBy = "Admin",
            Tags = request.Tags
        };

        await UploadImageAsync(request.Image!);

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return true;
    }

    // xóa post theo id
    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return false;

        File.Delete(@"wwwroot\images\" + post.Image);
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }

    // tìm bài viết theo id
    public async Task<Post?> FindPostByIdAsync(int id) => await _context.Posts.FindAsync(id);

    // hiển thị danh sách bài viết
    public async Task<List<Post>?> ShowPostsAsync(int currentPage)
    {
        var listPosts = await _context.Posts
            .Skip((currentPage - 1) * _pageSize)
            .Take(_pageSize)
            .ToListAsync();

        return listPosts;
    }

    public async Task<List<Post>?> ShowPreviousPosts(int currentPage)
    {
        var listPosts = await _context.Posts
            .Skip((currentPage - 1) * _pageSize)
            .Take(_pageSize)
            .ToListAsync();

        return listPosts;
    }

    public async Task<List<Post>?> ShowNextPosts(int currentPage)
    {
        var listPosts = await _context.Posts
            .Skip((currentPage - 1) * _pageSize)
            .Take(_pageSize)
            .ToListAsync();

        return listPosts;
    }

    public int NumberOfPages()
    {
        int getAllPosts = _context.Posts.Count();

        if (getAllPosts % _pageSize == 0)
            return getAllPosts / _pageSize;
        else
            return getAllPosts / _pageSize + 1;
    }

    // cập nhật bài viết
    public async Task<bool> UpdatePostAsync(int id, PostDTO request)
    {
        var postUpdate = await _context.Posts.FindAsync(id);

        if (postUpdate == null)
            return false;

        string oldImage = postUpdate.Image; // đường dẫn của hình ảnh cũ

        if (request.Image is not null)
        {
            File.Delete(@"wwwroot\images\" + oldImage); // xóa ảnh cũ
            postUpdate.Image = SetFileName(request.Image);
            await UploadImageAsync(request.Image);
        }

        postUpdate.Title = request.Title.ToUpper();
        postUpdate.Content = request.Content;
        postUpdate.LastUpdatedAt = DateTime.Now;
        postUpdate.Tags = request.Tags;

        _context.Posts.Update(postUpdate);
        await _context.SaveChangesAsync();

        return true;
    }

    private static async Task UploadImageAsync(IFormFile file)
    {
        if (file != null)
        {
            var filePath = Path.Combine("wwwroot", "images", SetFileName(file));

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }
    }

    private static string SetFileName(IFormFile file) =>
        DateTime.Now.ToString("yyyyMMddhhmmsstt") + "_" + Path.GetFileName(file.FileName);

    public async Task<List<Post>> SearchPostsAsync(string keyword)
    {
        var listSearchedPosts = await _context.Posts
            .Where(p =>
                p.Title.ToLower().Contains(keyword.ToLower()) ||
                p.Tags.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();

        return listSearchedPosts;
    }
}