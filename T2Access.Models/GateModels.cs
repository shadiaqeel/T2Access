using System;
using System.ComponentModel.DataAnnotations;

using T2Access.Models.Resources;

namespace T2Access.Models
{

    public interface IGateModel 
    {
         Guid Id { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]
         string UserName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Password))]
         string Password { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameAr))]
         string NameAr { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameEn))]
         string NameEn { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Status))]
         int? Status { get; set; }


    }



    public class GateModel : BaseModel , IGateModel
    {
        public Guid Id { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameAr))]
        public string NameAr { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameEn))]
        public string NameEn { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Status))]
        public int? Status { get; set; }
        public string Password { get ; set ; }
    }

    //=====================================================================================================

    public class CheckedGateModel : GateModel
    {
        public bool Checked { get; set; }

    }


    public class GateFilterModel : GateModel
    {
        public int? PageSize { get; set; }
        public int? Skip { get; set; }
    }








    //=====================================================================================================


    public class GateSignUpModel : IGateModel, IAuthModel
    {

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [MinLength(8, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "MoreThen")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]

          public  string UserName { get; set; }

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
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameAr))]

         public string NameAr { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "LessThen")]
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameEn))]

         public string NameEn { get; set; }
        public Guid Id { get; set; } = Guid.Empty;
        public int? Status { get; set; } = 0;
    }

    //=====================================================================================================




}
