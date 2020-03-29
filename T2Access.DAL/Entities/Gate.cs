using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.DAL
{
    public class Gate: BaseEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? Status { get; set; }
    }
}
