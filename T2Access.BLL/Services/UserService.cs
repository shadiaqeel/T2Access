using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        private readonly IUserManager userManager;
        private readonly IUserGateManager userGateManager;

        public UserService(IUserManager userManager = null, IUserGateManager userGateManager = null)
        {
            this.userManager = userManager ?? ManagerFactory.GetUserManager();
            this.userGateManager = userGateManager ?? ManagerFactory.GetUserGateManager();
        }



        //==========================================================================



        public async Task<ServiceResponse<string>> CreateAsync(SignUpUserModel model)
        {
            #region Removed
            //// Check user if exist 
            //if (await userManager.GetByUserNameAsync(model.UserName) != null)
            //{
            //    return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };
            //} 
            #endregion

            try
            {


                // Create  new user  
                Guid id = (await userManager.CreateAsync(model.ToEntity())).Id;
                if (id == Guid.Empty)
                {
                    return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupFailed };
                }

                // Check if there attached gates list  
                if (string.IsNullOrEmpty(model.AddedGateList))
                {
                    return new ServiceResponse<string>() { Data = Resource.SignupSuccess };
                }

                // assign gate for new user
                foreach (string gate in model.AddedGateList.Split(','))
                {
                    if (Guid.TryParse(gate, out Guid gateId))
                    {
                        await userGateManager.CreateAsync(new UserGate() { UserId = id, GateId = gateId });
                    }
                }

                return new ServiceResponse<string>() { Data = Resource.SignupSuccess };

            }
            catch (MySql.Data.MySqlClient.MySqlException error)
            {
                Trace.WriteLine($"(MySqlException)  {error.GetType()}   :    {error}  ");

                switch (error.Number)
                {
                    case 1062:
                        return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };
                    default:
                        return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupFailed };
                }

            }
            catch (System.Data.SqlClient.SqlException error)
            {
                Trace.WriteLine($"(SqlException)  {error.GetType()}   :    {error}  ");

                switch (error.Number)
                {
                    case 2627:
                        return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UserExist };
                    default:
                        return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupFailed };
                }

            }
            catch (Exception error)
            {
                Trace.WriteLine($"(Exception) {error.GetType()}   :    {error}  ");
                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.SignupFailed };
            }


        }



        public async Task<ServiceResponse<string>> EditAsync(UpdateUserModel model)
        {

            try
            {
                await userManager.UpdateAsync(model.ToEntity());

                //Delete records
                if (!string.IsNullOrEmpty(model.RemovedGateList))
                {
                    foreach (string gate in model.RemovedGateList.Split(','))
                    {
                        if (Guid.TryParse(gate, out Guid gateId))
                        {
                            /*await*/
                            userGateManager.DeleteAsync(new UserGate() { UserId = model.Id, GateId = gateId });
                        }
                    }
                }

                //Add records
                if (!string.IsNullOrEmpty(model.AddedGateList))
                {
                    foreach (string gate in model.AddedGateList.Split(','))
                    {
                        if (Guid.TryParse(gate, out Guid gateId))
                        {
                            /*await*/
                            userGateManager.CreateAsync(new UserGate() { UserId = model.Id, GateId = gateId });
                        }
                    }
                }

                Task.WaitAll();
            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }

            return new ServiceResponse<string>() { Data = Resource.EditSuccess };


        }



        public async Task<ServiceResponse<UserListResponse>> GetListAsync(FilterUserModel filter)
        {
            IEnumerable<User> userList;
            try
            {
                userList = await userManager.GetWithFilterAsync(filter.ToEntity());
            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<UserListResponse>() { Success = false, ErrorMessage = error.Message };

            }

            var _totalSize = userList.Count();

            //sorting 
            if (!string.IsNullOrEmpty(filter.Order))
            {
                userList = userList.AsQueryable().OrderBy(filter.Order);
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
                // remove all corresponding recorders for the User in UserGate Table
                await userGateManager.DeleteAsync(new UserGate() { UserId = id });

                await userManager.DeleteAsync(new User() { Id = id });

            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.DeleteFailed };
            }

            return new ServiceResponse<string>() { Data = Resource.DeleteSuccess };


        }



        public async Task<ServiceResponse<UserDto>> GetByIdAsync(Guid userId)
        {
            try
            {
                return new ServiceResponse<UserDto>() { Data = (await userManager.GetByIdAsync(userId)).ToDto() };
            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<UserDto>() { Success = false, ErrorMessage = error.Message };

            }

        }



        public async Task<ServiceResponse<UserDto>> LoginAsync(LoginModel model)
        {
            User user;
            try
            {
                user = await userManager.LoginAsync(model);

            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");
                return new ServiceResponse<UserDto>() { Success = false, ErrorMessage = Resource.OperationFailed };

            }

            return user == null ?
                  new ServiceResponse<UserDto>() { Success = false, ErrorMessage = Resource.UserNotExist } :
                  new ServiceResponse<UserDto>() { Data = user.ToDto() };



        }



        public async Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                await userManager.ResetPasswordAsync(model);

            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.EditFailed };
            }

            return new ServiceResponse<string>() { Data = Resource.EditSuccess };


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
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = error.Message };

            }


        }



        public async Task<ServiceResponse<string>> UnassignAsync(UserGateModel userGate)
        {

            try
            {
                await userGateManager.DeleteAsync(userGate.ToEntity());

                return new ServiceResponse<string>() { Data = Resource.UnassignSuccess };

            }
            catch (Exception error)
            {
                Trace.WriteLine($" {error.GetType()}   :    {error.Message }  ");

                return new ServiceResponse<string>() { Success = false, ErrorMessage = Resource.UnassignFailed };

            }




        }






    }


}

