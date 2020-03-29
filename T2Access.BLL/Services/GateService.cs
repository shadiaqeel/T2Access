using System;
using System.Collections.Generic;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.BLL.Extensions;


namespace T2Access.BLL.Services
{



    public class GateService : IGateService
    {

        private IGateManager gateManager = ManagerFactory.GetGateManager(Variables.DatabaseProvider);


        public bool Create(GateSignUpModel gateModel)
        {

            return gateManager.Create(gateModel);

        }

        public bool Edit(GateModel model)
        {
            return gateManager.Update(model);
        }

        public GateListResponse GetListWithFilter(GateFilterModel filter)
        {

            return gateManager.GetWithFilter(filter);

        }

        public List<CheckedGateModel> GetCheckedListByUserId(Guid userId)
        {

            return gateManager.GetCheckedByUserId(userId);

        }

        public bool CheckUserName(string userName)
        {
            return gateManager.GetByUserName(userName) == null ? true : false;

        }


        public GateModel Login(LoginModel gateModel)
        {

            return gateManager.Login(gateModel);


        }

        public bool Delete(Guid id)
        {
            return gateManager.Delete(id);
        }  
        
        public bool ResetPassword(ResetPasswordModel model)
        {
            return gateManager.ResetPassword(model);
        }


    }
}
