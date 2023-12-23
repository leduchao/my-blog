using MyBlog.Models;

namespace MyBlog
{
    public class PostHandler
    {
        private readonly IRepository<Post> _postsRepository;

        public PostHandler(IRepository<Post> postsRepository)
        {
            _postsRepository = postsRepository;
        }
    }
}