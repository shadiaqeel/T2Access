using System;
using System.ComponentModel.DataAnnotations;

using T2Access.Models.Resources;

namespace T2Access.Models
{





    public class UserModel : BaseModel
    {
        [Key]
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.FirstName))]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.LastName))]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Status))]
        public int? Status { get; set; } = 0;

    }
    //=====================================================================================================

    public class UpdateUserModel : UserModel
    {
        public string GateList { get; set; }
        public string UnselecedGateList { get; set; }

    }

    //=====================================================================================================


    public class FilterUserModel : UserModel
    {

        public int? PageSize { get; set; }
        public int? Skip { get; set; }
        public string Order { get; set; }

    }


    //=====================================================================================================



    public class SignUpUserModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "MoreThen")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "MoreThen")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Password))]
        [DataType(DataType.Password)]

        public string Password { get; set; }



        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "MoreThen")]
        [Compare("Password", ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "Mismatch")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.ConfirmPassword))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.FirstName))]


        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.LastName))]

        public string LastName { get; set; }


        public string GateList { get; set; }


    }
}
