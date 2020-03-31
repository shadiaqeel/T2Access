using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

using T2Access.BLL.Extensions;
using T2Access.BLL.Resources;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.BLL.Services
{



    public class GateService : IGateService
    {

        private IGateManager gateManager = ManagerFactory.GetGateManager(Variables.DatabaseProvider);




        public ServiceResponse<string> Create(SignUpGateModel gateModel)
        {

            try
            {
                if (gateManager.Create(gateModel.ToEntity()) != null)
                    return new ServiceResponse<string>() { Data = Resource.SignupSuccess };

                return new ServiceResponse<string>() { Success = false, Message = Resource.SignupSuccess };
            }
            catch (Exception ex)
            {

                return new ServiceResponse<string>() { Success = false, Message = ex.Message };
            }


        }

        //==================================================================================
        public ServiceResponse<string> Edit(GateModel model)
        {

            try
            {
                if (gateManager.Update(model.ToEntity()))
                    return new ServiceResponse<string>() { Data = Resource.EditSuccess };
                return new ServiceResponse<string>() { Success = false, Message = Resource.EditFailed };
            }
            catch (Exception ex)
            {

                return new ServiceResponse<string>() { Success = false, Message = ex.Message };
            }
        }
        //==================================================================================
        public ServiceResponse<GateListResponse> GetListWithFilter(FilterGateModel filter)
        {


            var gateList = gateManager.GetWithFilter(filter.ToEntity());


            var _totalSize = gateList.Count;

            //sorting 
            if (!string.IsNullOrEmpty(filter.Order))
                gateList = gateList.OrderBy(filter.Order).ToList<Gate>();

            //paging

            if (filter.Skip != null && filter.PageSize != null)
                gateList = gateList.Skip((int)filter.Skip).Take((int)filter.PageSize).ToList<Gate>();


           
            return new ServiceResponse<GateListResponse>() { Data = new GateListResponse() { ResponseList = gateList.ToDto(), totalEntities = _totalSize } };





        }
        //==================================================================================
        public ServiceResponse<IList<CheckedGateModel>> GetCheckedListByUserId(Guid userId)
        {


            return new ServiceResponse<IList<CheckedGateModel>>() { Data = gateManager.GetCheckedByUserId(userId) };

        }
        //==================================================================================

        public ServiceResponse<GateDto> Login(LoginModel gate)
        {

           return new ServiceResponse<GateDto>() { Data = gateManager.Login(gate).ToDto() };

        }
        //==================================================================================
        public ServiceResponse<string> Delete(Guid id)
        {
            try{
                if (gateManager.Delete(new Gate() { Id = id }))
                    return new ServiceResponse<string>() { Data = Resource.DeleteSuccess };
                return new ServiceResponse<string>() { Success = false, Message = Resource.DeleteFailed };
            }
            catch (Exception ex)
            {

                return new ServiceResponse<string>() { Success = false, Message = ex.Message };
            }





        }
        //==================================================================================
        public ServiceResponse<string> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (gateManager.ResetPassword(model))
                    return new ServiceResponse<string>() { Data = Resource.EditSuccess };
                return new ServiceResponse<string>() { Success = false, Message = Resource.EditFailed };
            }
            catch (Exception ex)
            {

                return new ServiceResponse<string>() { Success = false, Message = ex.Message };
            }

        }
        //==================================================================================

        private bool CheckUserName(string userName)
        {

            return gateManager.GetByUserName(userName) == null ? true : false;



        }

    }
}
