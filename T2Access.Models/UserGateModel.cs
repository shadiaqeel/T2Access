using System;
using System.ComponentModel.DataAnnotations;

using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class UserGateModel : BaseModel
    {

        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        public Guid UserId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ModelResource), ErrorMessageResourceName = "RequiredField")]
        public Guid GateId { get; set; }
    }
}
