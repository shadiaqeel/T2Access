using System;
using System.ComponentModel.DataAnnotations;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class LoginModel : BaseModel, IAuthModel
    {
        public Guid Id { get ; set ; } 

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "MoreThen")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "MoreThen")]
        [DataType(DataType.Password)]
        public string Password { get ; set ; }
    }
}
