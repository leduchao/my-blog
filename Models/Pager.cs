namespace MyBlog.Models;

public class Pager
{
    public int TotalPage { get; set; }
    public int PageSize { get; }
    public int CurrentPage { get; set; }

    public Pager()
    {
        PageSize = 6;
    }

    public Pager(int totalPage, int currentPage)
    {
        TotalPage = totalPage;
        CurrentPage = currentPage;
        PageSize = 6;
    }

}