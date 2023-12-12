using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Areas.Identity.DataContext;
using MyBlog.Areas.Identity.Models;
using MyBlog.Areas.Identity.Services;
using MyBlog.Services;
using MyBlog.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// add sqlite migration database
builder.Services.AddDbContext<MyBlogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyBlog")));

// add sqlite identity database
builder.Services.AddDbContext<BlogUserContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyBlog")));

builder.Services.AddIdentity<BlogUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false; // ky tu dac biet
    options.Password.RequireDigit = false; // ky tu so
    options.Password.RequireUppercase = false; // ky tu in hoa
    options.Password.RequireLowercase = false; // ky tu thuong
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<BlogUserContext>()
    .AddDefaultTokenProviders();

//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// add account service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPostsService, PostsService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/identity/account/access-denied";
    options.LoginPath = "/identity/account/login";
    options.LogoutPath = "/identity/account/logout";
});

// add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
