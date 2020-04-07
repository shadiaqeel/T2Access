using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                if (gateManager.GetByUserName(gateModel.UserName) != null)
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

        //==========================================================================================================
        public ServiceResponse<string> Edit(GateModel model)
        {
            try
            {
                gateManager.Update(model.ToEntity());

            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }
            return new ServiceResponse<string>() { Data = Resource.EditSuccess };

        }

        public ServiceResponse<GateListResponse> GetListWithFilter(FilterGateModel filter)
        {

            IEnumerable<Gate> AddedGateList;

            try
            {
                AddedGateList = gateManager.GetWithFilter(filter.ToEntity());

            }
            catch (Exception e)
            {

                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");
                return new ServiceResponse<GateListResponse>() { Success = false, ErrorMessage = Resource.OperationFailed };
            }

            var _totalSize = AddedGateList.Count();

            //sorting
            if (!string.IsNullOrEmpty(filter.Order))
            {
                AddedGateList = AddedGateList.OrderBy(filter.Order);
            }

            //paging

            if (filter.Skip != null && filter.PageSize != null)
            {
                AddedGateList = AddedGateList.Skip((int)filter.Skip).Take((int)filter.PageSize);
            }

            return new ServiceResponse<GateListResponse>() { Data = new GateListResponse() { ResponseList = AddedGateList.ToDto(), TotalEntities = _totalSize } };
        }



        public ServiceResponse<ListResponse<CheckedGateDto>> GetCheckedListByUserId(FilterUserModel filter)
        {

            IEnumerable<CheckedGateDto> AddedGateList;
            ListResponse<CheckedGateDto> response = new ListResponse<CheckedGateDto>();

            try
            {
                AddedGateList = gateManager.GetCheckedByUserId(filter.Id);
                response.TotalEntities = AddedGateList.Count();
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");
                return new ServiceResponse<ListResponse<CheckedGateDto>>() { Success = false, ErrorMessage = Resource.OperationFailed };
            }

            //search
            if (!string.IsNullOrEmpty(filter.SearchValue))
            {
                filter.SearchValue = filter.SearchValue.ToLower();

                AddedGateList = AddedGateList.Where(x => x.NameAr.ToLower().Contains(filter.SearchValue)
                                || x.NameEn.ToLower().Contains(filter.SearchValue)
                );

            }


            //sorting
            if (!string.IsNullOrEmpty(filter.Order))
            {
                AddedGateList = AddedGateList.OrderBy(filter.Order);
            }

            //paging

            if (filter.Skip != null && filter.PageSize != null)
            {
                AddedGateList = AddedGateList.Skip((int)filter.Skip).Take((int)filter.PageSize);
            }

            response.ResponseList = AddedGateList;

            return new ServiceResponse<ListResponse<CheckedGateDto>>() { Data = response };
        }




        public ServiceResponse<GateDto> Login(LoginModel gate)
        {
            return new ServiceResponse<GateDto>() { Data = gateManager.Login(gate).ToDto() };
        }

        public ServiceResponse<string> Delete(Guid id)
        {
            try
            {
                gateManager.Delete(new Gate() { Id = id });
                return new ServiceResponse<string>() { Data = Resource.DeleteSuccess };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.DeleteFailed };
            }
        }

        public ServiceResponse<string> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                gateManager.ResetPassword(model);
                return new ServiceResponse<string>() { Data = Resource.EditSuccess };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };

            }
        }
    }

}
