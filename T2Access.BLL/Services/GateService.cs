using System;
using System.Collections.Generic;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.BLL.Extensions;
using System.Linq;

namespace T2Access.BLL.Services
{



    public class GateService : IGateService
    {

        private IGateManager gateManager = ManagerFactory.GetGateManager(Variables.DatabaseProvider);


        public bool Create(GateSignUpModel gateModel)
        {

            return gateManager.Create(gateModel.ToEntity()) == null ? false : true ;

        }

        public bool Edit(IGateModel model)
        {
            return gateManager.Update(model.ToEntity());
        }

        public GateListResponse GetListWithFilter(GateFilterModel filter)
        {


            var gateList =  gateManager.GetWithFilter(filter.ToEntity());


            var _totalSize = gateList.Count;



            //paging

            if (filter.Skip != null && filter.PageSize != null)
                gateList = gateList.Skip((int)filter.Skip).Take((int)filter.PageSize).ToList<Gate>();




            return new GateListResponse() { ResponseList = gateList.ToModel(), totalEntities = _totalSize };

        }

        public IList<CheckedGateModel> GetCheckedListByUserId(Guid userId)
        {

            return gateManager.GetCheckedByUserId(userId);

        }

        public bool CheckUserName(string userName)
        {
            return gateManager.GetByUserName(userName) == null ? true : false;

        }


        public IGateModel Login(LoginModel gateModel)
        {

            return gateManager.Login(gateModel).ToModel();


        }

        public bool Delete(Guid id)
        {
            return gateManager.Delete(new Gate() { Id = id});
        }  
        
        public bool ResetPassword(ResetPasswordModel model)
        {
            return gateManager.ResetPassword(model);
        }


    }
}
