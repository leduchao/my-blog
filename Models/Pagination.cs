using MyBlog.Models;

namespace MyBlog
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public Pagination(List<Post> items, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling((double)items.Count / pageSize);
            PageSize = pageSize;
        }

        public bool HasNextPage => CurrentPage < TotalPages;

        public bool HasPreviousPage => CurrentPage > 1;

        public List<Post> CreatePaginatedList(List<Post> source)
        {
            return source
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}