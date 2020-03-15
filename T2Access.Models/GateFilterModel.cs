using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public class GateFilterModel: BaseModel
    {

        public string UserName  { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int?  Status { get; set; }

    }
}
