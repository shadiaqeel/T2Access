﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class UserModel: BaseModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(150, ErrorMessage = "{0} must be less than {1}")]
        public string UserName  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int  Status { get; set; }

    }
}
