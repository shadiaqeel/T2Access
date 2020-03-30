using System;
using System.Collections.Generic;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.BLL.Extensions;
using System.Linq;

namespace T2Access.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager userManager = ManagerFactory.GetUserManager(Variables.DatabaseProvider);
        private readonly IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);






        public bool Create(UserSignUpModel model)
        {

            Guid id = (userManager.Create(model.ToEntity())).Id;

            if (id == Guid.Empty)
                return false;

            if (string.IsNullOrEmpty(model.GateList))
                return true;


            Guid gateId;
            var gatelist = model.GateList.Split(',');
            foreach (string gate in gatelist)
            {
                if (Guid.TryParse(gate, out gateId))
                    userGateManager.Create(new UserGate() { UserId = id, GateId = gateId });
            }

            return true;

        }


        public bool Edit(UserUpdateModel model)
        {


            if (!userManager.Update(model.ToEntity()))
                return false;


            //Clear previous records
            userGateManager.Delete(new UserGate() { UserId = model.Id });


            if (string.IsNullOrEmpty(model.GateList))
                return true;
            //Create new records
            Guid gateId;
            var gatelist = model.GateList.Split(',');
            foreach (string gate in gatelist)
            {
                if (Guid.TryParse(gate, out gateId))
                    userGateManager.Create(new UserGate() { UserId = model.Id, GateId = gateId });
            }


            return true;

        }


        public UserListResponse GetList(UserFilterModel filter)
        {

            var userList =  userManager.GetWithFilter(filter.ToEntity());

            var _totalSize = userList.Count;

            //paging
            if (filter.Skip != null && filter.PageSize != null)
                userList = userList.Skip((int)filter.Skip).Take((int)filter.PageSize).ToList<User>();

            return new UserListResponse () { ResponseList = userList.ToModel(), totalEntities = _totalSize };



        }



        public bool Delete(Guid id)
        {
            if (userGateManager.Delete(new UserGate() { UserId = id }))
            {

                return userManager.Delete(new User() { Id = id }) ;

            }

            return false; 
        }



        public bool CheckUserName(string userName) {

            return userManager.GetByUserName(userName) == null ? true : false;

        }    
        
        public IUserModel GetById(Guid userId) {

            return userManager.GetById(userId).ToModel();

        }


        public IUserModel Login(LoginModel user)
        {

         return userManager.Login(user).ToModel();

        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            return userManager.ResetPassword(model);
        }


        public bool Assign(UserGateModel userGate)
        {

            return userGateManager.Create(userGate.ToEntity()) == null ? false : true;
        }



        public bool Unassign(UserGateModel userGate)
        {
            return userGateManager.Delete(userGate.ToEntity());

        }






    }


}

