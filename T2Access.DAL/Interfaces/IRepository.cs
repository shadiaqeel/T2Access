namespace T2Access.DAL
{
    public interface IRepository<T>

    {
        T Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);



    }
}
