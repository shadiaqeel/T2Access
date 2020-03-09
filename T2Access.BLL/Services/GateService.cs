using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;
using T2Access.DAL;

namespace T2Access.BLL.Services.Impl
{


    class GateService : IGateService
    {

        private IGateManager gateManager = new GateManager();

        public bool Create(Gate gate)
        {

            return gateManager.Insert(gate);



        }



        public List<Gate> List(Gate gate)
        {

           return gateManager.GetWithFilter(gate);

        }



    }
}
