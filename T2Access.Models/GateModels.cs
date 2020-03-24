using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class GateModel: BaseModel
    {
        public Guid Id { get; set; }


        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.NameAr))]
        public string NameAr { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.NameEn))]
        public string NameEn { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Status))]
        public int? Status { get; set; }
    }

    //=====================================================================================================



    public class GateFilterModel : BaseModel
    {

        public string UserName { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? Status { get; set; }

    }








    //=====================================================================================================


    public class GateSignUpModel : BaseModel, IAuthModel
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
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.NameAr))]

        public string NameAr { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LessThen")]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.NameEn))]

        public string NameEn { get; set; }


    }

    //=====================================================================================================




}
