using System;
using System.Collections.Generic;
using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager userManager = new UserManager();
        private readonly IUserGateManager userGateManager = new UserGateManager();


        public bool Create(UserSignUpModel user)
        {

            //UserModel _user = new UserModel();
            //_user.UserName = user.UserName;
            //_user.Password = user.Password;
            //_user.FirstName = user.FirstName;
            //_user.LastName= user.LastName;

            return userManager.Insert(user);
        }

        public bool CheckUserName(string userName) {


            return userManager.GetByUserName(userName) == null ? true : false;

        }

        //public bool CheckStatus(string userName)
        //{
        //    User user = userManager.GetByUserName(userName);

        //    user.

        //    return   

        
        //}
        public UserModel Login(LoginModel user)
        {

         return userManager.Login(user);

        }
        public List<UserModel> GetList(UserModel user)
        {
            return userManager.GetWithFilter(user);
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

