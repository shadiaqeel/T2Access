using System.ComponentModel.DataAnnotations;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class LoginModel : BaseModel, IAuthModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [DataType(DataType.Password)]
        public string Password { get ; set ; }
    }
}
