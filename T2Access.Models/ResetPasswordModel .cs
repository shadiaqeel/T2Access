using System;
using System.ComponentModel.DataAnnotations;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class ResetPasswordModel : BaseModel,IAuthModel
    {

        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
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
    }
}
