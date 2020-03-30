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
        public static User ToEntity(this IUserModel model)
        {
            return new User {

                Id = model.Id,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Status = model.Status

            };
        }

        public static IUserModel ToModel(this User user)
        {

            return new UserModel
            {

                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = (int)user.Status

            };
        }

        public static IList<IUserModel> ToModel(this IList<User> user)
        {

            return user.Select(c => c.ToModel()).ToList<IUserModel>();

        }
        #endregion
    }
}
