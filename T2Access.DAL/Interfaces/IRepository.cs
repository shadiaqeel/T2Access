using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IRepository<T> 

    {
        T Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);



    }
}
