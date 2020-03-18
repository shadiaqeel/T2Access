using System.ComponentModel.DataAnnotations;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class UserSignUpModel: BaseModel, IAuthModel
    {

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [Display( ResourceType =typeof(Resource) ,Name=nameof(Resource.UserName))]
        public string UserName { get; set; }
         
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Password))]

        public string Password { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Mismatch")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.ConfirmPassword))]

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
