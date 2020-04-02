using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Extensions
{
    public static class UserExtension
    {

        #region mapper




        public static User ToEntity(this SignUpUserModel user)
        {
            return new User
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password

            };
        
        }


        //public static User ToEntity(this UpdateUserModel user)
        //{
        //    return new User
        //    {
        //        Id = user.Id,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Status = user.Status

        //    };

        //}





        public static User ToEntity(this UserModel model)
        {
            return new User {

                Id = model.Id,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Status = model.Status

            };
        }

        public static UserDto ToDto(this User user)
        {

            return new UserDto
            {

                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = (int)user.Status

            };
        }

        public static IEnumerable<UserDto> ToDto(this IEnumerable<User> user)
        {

            return user.Select(c => c.ToDto());

        }
        #endregion
    }
}
