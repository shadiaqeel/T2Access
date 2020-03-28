using System;
using System.Collections.Generic;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager userManager = ManagerFactory.GetUserManager(Variables.DatabaseProvider);
        private readonly IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);






        public bool Create(UserSignUpModel user)
        {

            return userManager.Create(user);
        }

        public ResponseFilteredUserList GetList(UserFilterModel filter)
        {
            return userManager.GetWithFilter(filter);
        }

        public bool Edit(UserUpdateModel model)
        {
            return userManager.Update(model);
        }

        public bool Delete(Guid id)
        {
            return userManager.Delete(id);
        }



        public bool CheckUserName(string userName) {


            return userManager.GetByUserName(userName) == null ? true : false;

        }    
        
        public UserModel GetById(Guid userId) {


            return userManager.GetById(userId);

        }


        public UserModel Login(LoginModel user)
        {

         return userManager.Login(user);

        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            return userManager.ResetPassword(model);
        }


        public bool Assign(UserGateModel userGate)
        {

            return userGateManager.Create(userGate);
        }



        public bool Unassign(UserGateModel userGate)
        {
            return userGateManager.Delete(userGate);

        }






    }


}

