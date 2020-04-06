namespace T2Access.DAL
{
    public interface IRepository<T>

    {
        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);



    }
}
