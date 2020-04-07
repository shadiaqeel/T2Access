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

namespace T2Access.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager userManager = ManagerFactory.GetUserManager(Variables.DatabaseProvider);
        private readonly IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);




        //==========================================================================

        public ServiceResponse<string> Create(SignUpUserModel model)
        {



            if (userManager.GetByUserName(model.UserName) != null)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };
            }

            Guid id = (userManager.Create(model.ToEntity())).Id;

            if (id == Guid.Empty)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupFailed };
            }

            if (string.IsNullOrEmpty(model.AddedGateList))
            {
                return new ServiceResponse<string>() { Data = Resource.SignupSuccess };
            }

            foreach (string gate in model.AddedGateList.Split(','))
            {
                if (Guid.TryParse(gate, out Guid gateId))
                {
                    userGateManager.Create(new UserGate() { UserId = id, GateId = gateId });
                }
            }

            return new ServiceResponse<string>() { Data = Resource.SignupSuccess };

        }



        public ServiceResponse<string> Edit(UpdateUserModel model)
        {


            try
            {
                userManager.Update(model.ToEntity());

                //Clear previous records
                var deletedGateList = model.RemovedGateList?.Split(',');
                if (deletedGateList != null)
                {
                    foreach (string gate in deletedGateList)
                    {
                        if (Guid.TryParse(gate, out Guid gateId))
                        {
                            userGateManager.Delete(new UserGate() { UserId = model.Id, GateId = gateId });
                        }
                    }
                }

                if (string.IsNullOrEmpty(model.AddedGateList))
                {
                    return new ServiceResponse<string>() { Data = Resource.EditSuccess };
                }


                //Create new records
                var AddedGateList = model.AddedGateList.Split(',');
                foreach (string gate in AddedGateList)
                {
                    if (Guid.TryParse(gate, out Guid gateId))
                    {
                        userGateManager.Create(new UserGate() { UserId = model.Id, GateId = gateId });
                    }
                }

                return new ServiceResponse<string>() { Data = Resource.EditSuccess };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }

        }



        public ServiceResponse<UserListResponse> GetList(FilterUserModel filter)
        {
            IEnumerable<User> userList;
            try
            {
                userList = userManager.GetWithFilter(filter.ToEntity());
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<UserListResponse>() { ErrorMessage = e.Message };

            }

            var _totalSize = userList.Count();

            //sorting 
            if (!string.IsNullOrEmpty(filter.Order))
            {
                userList = userList.OrderBy(filter.Order);
            }


            //paging
            if (filter.Skip != null && filter.PageSize != null)
            {
                userList = userList.Skip((int)filter.Skip).Take((int)filter.PageSize);
            }

            return new ServiceResponse<UserListResponse>() { Data = new UserListResponse() { ResponseList = userList.ToDto(), TotalEntities = _totalSize } };



        }



        public ServiceResponse<string> Delete(Guid id)
        {

            try
            {
                userGateManager.Delete(new UserGate() { UserId = id });

                // remove all corresponding recorders for the User in UserGate Table
                userManager.Delete(new User() { Id = id });

                return new ServiceResponse<string>() { Data = Resource.DeleteSuccess };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.DeleteFailed };
            }

        }



        public ServiceResponse<UserDto> GetById(Guid userId)
        {
            try
            {
                return new ServiceResponse<UserDto>() { Data = userManager.GetById(userId).ToDto() };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<UserDto>() { Success = false, ErrorMessage = e.Message };

            }

        }



        public ServiceResponse<UserDto> Login(LoginModel model)
        {
            var user = userManager.Login(model);

            return user != null ?
                  new ServiceResponse<UserDto>() { Data = user.ToDto() } :
                  new ServiceResponse<UserDto>() { Success = false, ErrorMessage = Resource.UserNotExist };



        }




        public ServiceResponse<string> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                userManager.ResetPassword(model);

                return new ServiceResponse<string>() { Data = Resource.EditSuccess };


            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }

        }



        public ServiceResponse<string> Assign(UserGateModel userGate)
        {


            try
            {
                if (userGateManager.Create(userGate.ToEntity()) != null)
                {
                    return new ServiceResponse<string>() { Data = Resource.AssignSuccess };
                }

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.AssignFailed };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = e.Message };

            }


        }



        public ServiceResponse<string> Unassign(UserGateModel userGate)
        {

            try
            {
                userGateManager.Delete(userGate.ToEntity());

                return new ServiceResponse<string>() { Data = Resource.UnassignSuccess };

            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UnassignFailed };

            }




        }






    }


}

