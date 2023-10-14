using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Areas.Identity.Models;

namespace MyBlog.Areas.Identity.DataContext;

public class BlogUserContext : IdentityDbContext<BlogUser>
{
    public BlogUserContext(DbContextOptions<BlogUserContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // loai bo tien to "AspNet" khi tao bang
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            tableName ??= "NoName";

            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }
}