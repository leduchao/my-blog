namespace MyBlog
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        IQueryable<T> GetQueryable();

        Task SaveChangesAsync();
    }
}