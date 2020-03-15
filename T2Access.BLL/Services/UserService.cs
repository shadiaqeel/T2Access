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



            return userManager.Insert(user);
        }

        public bool CheckUserName(string userName) {


            return userManager.GetByUserName(userName) == null ? true : false;

        }


        public UserModel Login(LoginModel user)
        {

         return userManager.Login(user);

        }
        public List<UserModel> GetList(UserFilterModel filter)
        {
            return userManager.GetWithFilter(filter);
        }

        public bool Assign(UserGateModel userGate)
        {

            return userGateManager.Insert(userGate);
        }



        public bool Unassign(UserGateModel userGate)
        {
            return userGateManager.Delete(userGate);

        }

    }


}

