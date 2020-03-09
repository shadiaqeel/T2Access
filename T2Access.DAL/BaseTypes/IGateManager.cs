﻿using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IGateManager :IRepository<Gate>
    {
        bool Insert(Gate gate);
        List<Gate> GetWithFilter(Gate gate);



    }
}