using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{
    public class Gate: BaseEntity
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int Status { get; set; }
    }
}
