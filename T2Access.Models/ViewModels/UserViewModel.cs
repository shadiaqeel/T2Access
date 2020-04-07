using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using T2Access.Models.Resources;
namespace T2Access.Models
{
    //[JsonConverter(typeof(StringEnumConverter))]

    public enum UserStatus
    {

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Active), GroupName = "blue")]
        [EnumMember(Value = nameof(ModelResource.Active))]

        Active = 0,

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Admin), GroupName = "green")]
        [EnumMember(Value = nameof(ModelResource.Admin))]
        Admin = 1,

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Block), GroupName = "red")]
        [EnumMember(Value = nameof(ModelResource.Block))]
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

        public string AddedGateList { get; set; }
        public string RemovedGateList { get; set; }



    }
}