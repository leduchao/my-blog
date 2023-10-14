using Microsoft.AspNetCore.Mvc.RazorPages;
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
        _pageSize = 6;
    }

    // tạo bài viết mới
    public async Task<bool> CreatePost(PostDTO request)
    {
        var newPost = new Post
        {
            Title = request.Title,
            Image = SetFileName(request.Image!),
            Content = request.Content,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
            CreatedBy = "Admin",
        };

        await UploadImageAsync(request.Image!);

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return true;
    }

    // xóa post theo id
    public async Task<bool> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return false;

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }

    // tìm bài viết theo id
    public async Task<Post?> FindPostById(int id) => await _context.Posts.FindAsync(id);

    // public int ToTalPage()
    // {
    //     var totalPosts = _context.Posts.ToList().Count;

    //     if (totalPosts < _pageSize)
    //         return 1;

    //     if (totalPosts % _pageSize != 0)
    //         return totalPosts / _pageSize + 1;

    //     return totalPosts / _pageSize;
    // }

    // hiển thị danh sách bài viết
    public async Task<List<Post>?> ShowPosts(int currentPage)
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

    // public async Task<List<Post>?> ShowNextPagePosts(int currentPage)
    // {
    //     return await _context.Posts
    //         .Skip(currentPage * _pageSize)
    //         .Take(_pageSize)
    //         .ToListAsync();
    // }

    // public async Task<List<Post>?> ShowLastPagePosts()
    // {
    //     var totalPage = ToTalPage();

    //     return await _context.Posts
    //         .Skip((totalPage - 1) * _pageSize)
    //         .Take(_pageSize)
    //         .ToListAsync();
    // }

    // cập nhật bài viết
    public async Task<bool> UpdatePost(int id, PostDTO request)
    {
        var postUpdate = await _context.Posts.FindAsync(id);

        if (postUpdate == null)
            return false;

        string oldImage = postUpdate.Image; // đường dẫn của hình ảnh cũ

        postUpdate.Title = request.Title;
        postUpdate.Image = SetFileName(request.Image!);
        postUpdate.Content = request.Content;
        postUpdate.LastUpdatedAt = DateTime.Now;

        File.Delete(@"wwwroot\images\" + oldImage); // xóa ảnh cũ
        await UploadImageAsync(request.Image!);

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

    private static string SetFileName(IFormFile file)
    {
        return DateTime.Now.ToString()
            .Replace(" ", "")
            .Replace("/", "")
            .Replace(":", "") + "_" + Path.GetFileName(file.FileName);
    }
}