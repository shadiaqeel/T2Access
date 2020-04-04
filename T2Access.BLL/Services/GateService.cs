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
        private readonly IGateManager gateManager = ManagerFactory.GetGateManager(Variables.DatabaseProvider);

        public ServiceResponse<string> Create(SignUpGateModel gateModel)
        {
            try
            {
                if (gateManager.GetByUserName(gateModel.UserName) == null)
                {
                    return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };
                }

                return gateManager.Create(gateModel.ToEntity()) != null
                    ? new ServiceResponse<string>() { Data = Resource.SignupSuccess }
                    : new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupSuccess };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };
            }
        }

        //==================================================================================
        public ServiceResponse<string> Edit(GateModel model)
        {
            try
            {
                return gateManager.Update(model.ToEntity())
                    ? new ServiceResponse<string>() { Data = Resource.EditSuccess }
                    : new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };
            }
        }

        public ServiceResponse<GateListResponse> GetListWithFilter(FilterGateModel filter)
        {
            var gateList = gateManager.GetWithFilter(filter.ToEntity());

            var _totalSize = gateList.Count();

            //sorting
            if (!string.IsNullOrEmpty(filter.Order))
            {
                gateList = gateList.OrderBy(filter.Order);
            }

            //paging

            if (filter.Skip != null && filter.PageSize != null)
            {
                gateList = gateList.Skip((int)filter.Skip).Take((int)filter.PageSize);
            }

            return new ServiceResponse<GateListResponse>() { Data = new GateListResponse() { ResponseList = gateList.ToDto(), totalEntities = _totalSize } };
        }

        public ServiceResponse<IEnumerable<CheckedGateDto>> GetCheckedListByUserId(Guid userId) => new ServiceResponse<IEnumerable<CheckedGateDto>>() { Data = gateManager.GetCheckedByUserId(userId) };

        public ServiceResponse<GateDto> Login(LoginModel gate) => new ServiceResponse<GateDto>() { Data = gateManager.Login(gate).ToDto() };

        public ServiceResponse<string> Delete(Guid id)
        {
            try
            {
                return gateManager.Delete(new Gate() { Id = id })
                    ? new ServiceResponse<string>() { Data = Resource.DeleteSuccess }
                    : new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.DeleteFailed };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };
            }
        }

        public ServiceResponse<string> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                return gateManager.ResetPassword(model)
                    ? new ServiceResponse<string>() { Data = Resource.EditSuccess }
                    : new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}