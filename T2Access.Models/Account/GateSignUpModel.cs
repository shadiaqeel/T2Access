using System.ComponentModel.DataAnnotations;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class GateSignUpModel:IAuthModel
    {

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessage = "{0} must be less than {1}")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessage = "{0} must be less than {1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessage = "{0} must be less than {1}")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="{0}")]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessage = "{0} must be less than {1}")]
        public string NameAr { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessage = "{0} must be less than {1}")]
        public string NameEn { get; set; }


    }
}
