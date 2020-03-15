using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class UserFilterModel: BaseModel
    {

        public string UserName  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int?  Status { get; set; }

    }
}
