using System.Threading.Tasks;

namespace T2Access.DAL
{
    public interface IRepository<T>

    {
        Task<T> CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);



    }
}
