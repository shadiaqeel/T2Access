﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IRepository<T,D> where T: class
    {
        bool Insert(T entity);

        bool Delete(D id);


    }
}
