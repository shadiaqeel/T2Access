﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGateModel>
    {
        // bool Insert(Guid userId, Guid gateId);
         bool Delete(UserGateModel userGate);
    }
}
