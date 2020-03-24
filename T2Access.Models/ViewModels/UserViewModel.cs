using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using T2Access.Models;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public enum UserStatus
    {
       
        Regular = 0,
        Admin = 1,
        Block = 2

    }

    public class UserViewModel 
    {
        public Guid Id { get; set; }


        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.FirstName))]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.LastName))]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Status))]
        public UserStatus Status { get; set; }


    }
}