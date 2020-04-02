using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

using T2Access.BLL.Extensions;
using T2Access.BLL.Resources;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager userManager = ManagerFactory.GetUserManager(Variables.DatabaseProvider);
        private readonly IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);




        //==========================================================================
        public ServiceResponse<string> Create(SignUpUserModel model)
        {



            if (CheckUserName(model.UserName))
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };



            Guid id = (userManager.Create(model.ToEntity())).Id;

            if (id == Guid.Empty)
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupFailed };


            if (string.IsNullOrEmpty(model.GateList))
                return new ServiceResponse<string>() { Data = Resource.SignupSuccess  };


            Guid gateId;
            var gatelist = model.GateList.Split(',');
            foreach (string gate in gatelist)
            {
                if (Guid.TryParse(gate, out gateId))
                    userGateManager.Create(new UserGate() { UserId = id, GateId = gateId });
            }

            return new ServiceResponse<string>() { Data = Resource.SignupSuccess };

        }

        //==========================================================================

        public ServiceResponse<string> Edit(UpdateUserModel model)
        {


            if (!userManager.Update(model.ToEntity()))
            {

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };

            }

            //Clear previous records
            userGateManager.Delete(new UserGate() { UserId = model.Id });


            if (string.IsNullOrEmpty(model.GateList))
                return new ServiceResponse<string>() { Data = Resource.EditSuccess };


            //Create new records
            Guid gateId;
            var gatelist = model.GateList.Split(',');
            foreach (string gate in gatelist)
            {
                if (Guid.TryParse(gate, out gateId))
                    userGateManager.Create(new UserGate() { UserId = model.Id, GateId = gateId });
            }


            return new ServiceResponse<string>() { Data = Resource.EditSuccess };

        }

        //==========================================================================
        public ServiceResponse<UserListResponse> GetList(FilterUserModel filter)
        {
            IEnumerable<User> userList;
            try
            {
                userList = userManager.GetWithFilter(filter.ToEntity());
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserListResponse>() { ErrorMessage = ex.Message };

            }

            var _totalSize = userList.Count();

            //sorting 
            if (!string.IsNullOrEmpty(filter.Order))
                userList = userList.OrderBy(filter.Order);


            //paging
            if (filter.Skip != null && filter.PageSize != null)
                userList = userList.Skip((int)filter.Skip).Take((int)filter.PageSize);

            return new ServiceResponse<UserListResponse>() { Data = new UserListResponse() { ResponseList = userList.ToDto(), totalEntities = _totalSize } };



        }

        //==========================================================================

        public ServiceResponse<string> Delete(Guid id)
        {
            if (userGateManager.Delete(new UserGate() { UserId = id }))
            {
                if (userManager.Delete(new User() { Id = id }))
                    return new ServiceResponse<string>() { Data = Resource.DeleteSuccess };


            }

            return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.DeleteFailed };

        }

        //==========================================================================

        private bool CheckUserName(string userName)
        {

            return userManager.GetByUserName(userName) != null ? true : false;

        }

        //==========================================================================

        public ServiceResponse<UserDto> GetById(Guid userId)
        {
            try
            {
                var user = userManager.GetById(userId).ToDto();

                return new ServiceResponse<UserDto>() { Data = user };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserDto>() { Success = false, ErrorMessage = ex.Message };

            }

        }
        //==========================================================================

        public ServiceResponse<UserDto> Login(LoginModel model)
        {
            var user = userManager.Login(model);

            return user != null ?
                  new ServiceResponse<UserDto>() { Data = user.ToDto() } :
                  new ServiceResponse<UserDto>() { Success = false, ErrorMessage = Resource.UserNotExist };



        }
        //==========================================================================

        public ServiceResponse<string> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (userManager.ResetPassword(model))
                    return new ServiceResponse<string>() { Data = Resource.EditSuccess };

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }
            catch (Exception ex)
            {

                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };
            }

        }
        //==========================================================================


        public ServiceResponse<string> Assign(UserGateModel userGate)
        {


            try
            {
                if (userGateManager.Create(userGate.ToEntity()) != null)
                    return new ServiceResponse<string>() { Data = Resource.AssignSuccess };

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.AssignFailed };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };

            }


        }

        //==========================================================================


        public ServiceResponse<string> Unassign(UserGateModel userGate)
        {

            try
            {
                if (userGateManager.Delete(userGate.ToEntity()))
                    return new ServiceResponse<string>() { Data = Resource.UnassignSuccess };

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UnassignFailed };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = ex.Message };

            }




        }






    }


}

