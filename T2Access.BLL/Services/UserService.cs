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

            User _user = new User();
            _user.UserName = user.UserName;
            _user.Password = user.Password;
            _user.FirstName = user.FirstName;
            _user.LastName= user.LastName;

            return userManager.Insert(_user);
        }

        public bool CheckUserName(string userName) {


            return userManager.GetByUserName(userName) == null ? true : false;



        }
        public User Login(LoginModel user)
        {

         return userManager.Login(user);

        }
        public List<User> GetList(User user)
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

