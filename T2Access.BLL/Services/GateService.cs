using System.Collections.Generic;
using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Services
{



    public class GateService : IGateService
    {

        private IGateManager gateManager = new GateManager();



        public bool Create(GateSignUpModel gateModel)
        {

            return gateManager.Insert(gateModel);

        }

        public List<GateModel> GetListWithFilter(GateModel gate)
        {

            return gateManager.GetWithFilter(gate);

        }


        public bool CheckUserName(string userName)
        {
            return gateManager.GetByUserName(userName) == null ? true : false;

        }


        public GateModel Login(LoginModel gateModel)
        {

            return gateManager.Login(gateModel);


        }
    }
}
