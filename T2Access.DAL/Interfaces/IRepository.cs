using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IRepository<C,D,U> 
        where C: class
        where U: class
    {
        bool Create(C itemModel);

        bool Update(U editmodel);

        bool Delete(D itemId);



    }
}
