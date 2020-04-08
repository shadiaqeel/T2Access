﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

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

        public async Task<ServiceResponse<string>> CreateAsync(SignUpUserModel model)
        {



            if (userManager.GetByUserNameAsync(model.UserName) != null)
            {
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };
            }

            Guid id = (await userManager.CreateAsync(model.ToEntity())).Id;

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
                    await userGateManager.CreateAsync(new UserGate() { UserId = id, GateId = gateId });
                }
            }

            return new ServiceResponse<string>() { Data = Resource.SignupSuccess };

        }



        public async Task<ServiceResponse<string>> EditAsync(UpdateUserModel model)
        {


            try
            {
                await userManager.UpdateAsync(model.ToEntity());

                //Clear previous records
                var deletedGateList = model.RemovedGateList?.Split(',');
                if (deletedGateList != null)
                {
                    foreach (string gate in deletedGateList)
                    {
                        if (Guid.TryParse(gate, out Guid gateId))
                        {
                            await userGateManager.DeleteAsync(new UserGate() { UserId = model.Id, GateId = gateId });
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
                        await userGateManager.CreateAsync(new UserGate() { UserId = model.Id, GateId = gateId });
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



        public async Task<ServiceResponse<UserListResponse>> GetListAsync(FilterUserModel filter)
        {
            IEnumerable<User> userList;
            try
            {
                userList = await userManager.GetWithFilterAsync(filter.ToEntity());
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



        public async Task<ServiceResponse<string>> DeleteAsync(Guid id)
        {

            try
            {
                await userGateManager.DeleteAsync(new UserGate() { UserId = id });

                // remove all corresponding recorders for the User in UserGate Table
                await userManager.DeleteAsync(new User() { Id = id });

                return new ServiceResponse<string>() { Data = Resource.DeleteSuccess };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.DeleteFailed };
            }

        }



        public async Task<ServiceResponse<UserDto>> GetByIdAsync(Guid userId)
        {
            try
            {
                return new ServiceResponse<UserDto>() { Data = (await userManager.GetByIdAsync(userId)).ToDto() };
            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<UserDto>() { Success = false, ErrorMessage = e.Message };

            }

        }



        public async Task<ServiceResponse<UserDto>> LoginAsync(LoginModel model)
        {
            User user;
            try
            {
                 user = await userManager.LoginAsync(model);

            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");
                return new ServiceResponse<UserDto>() { Success = false, ErrorMessage = Resource.OperationFailed };

            }
            return user != null ?
                  new ServiceResponse<UserDto>() { Data = user.ToDto() } :
                  new ServiceResponse<UserDto>() { Success = false, ErrorMessage = Resource.UserNotExist };



        }




        public async Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                await userManager.ResetPasswordAsync(model);

                return new ServiceResponse<string>() { Data = Resource.EditSuccess };


            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }

        }



        public async Task<ServiceResponse<string>> AssignAsync(UserGateModel userGate)
        {


            try
            {
                if (await userGateManager.CreateAsync(userGate.ToEntity()) != null)
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



        public async Task<ServiceResponse<string>> UnassignAsync(UserGateModel userGate)
        {

            try
            {
                await userGateManager.DeleteAsync(userGate.ToEntity());

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

