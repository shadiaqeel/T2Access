using System;
using System.ComponentModel.DataAnnotations;

using T2Access.Models.Resources;
namespace T2Access.Models
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum UserStatus
    {
        Regular = 0,
        Admin = 1,
        Block = 2

    }


    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.FirstName))]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.LastName))]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Status))]

        public UserStatus? Status { get; set; } = 0;

        public string GateList { get; set; }


    }
}