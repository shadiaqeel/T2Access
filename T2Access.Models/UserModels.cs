using System;
using System.ComponentModel.DataAnnotations;

using T2Access.Models.Resources;

namespace T2Access.Models
{





    public class UserModel : BaseModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MoreThen")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.FirstName))]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.LastName))]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Status))]
        public int? Status { get; set; }

    }

    //=====================================================================================================


    public class UserFilterModel : BaseModel
    {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Status { get; set; }

    }


    //=====================================================================================================



    public class UserSignUpModel : BaseModel, IAuthModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MoreThen")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UserName))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MoreThen")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Password))]
        [DataType(DataType.Password)]

        public string Password { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MoreThen")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Mismatch")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.ConfirmPassword))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.FirstName))]

        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.LastName))]

        public string LastName { get; set; }


    }
}
