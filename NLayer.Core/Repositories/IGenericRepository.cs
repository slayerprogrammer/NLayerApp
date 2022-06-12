using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);

        //IQueryable yazmamızın sebebi ek olarak orderby veya
        //başka sorgulamalar yapmak için daha veritabanına gitmediğimiz için.
        IQueryable<T> GetAll();

        //example: productRepository.Where(x=>x.Id > 5)
        //OrderBy.ToListAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
