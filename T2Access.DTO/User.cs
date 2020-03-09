using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{
    public class User: BaseEntity
    {
        public Guid Id { get; set; }
        public string Username  { get; set; }
        public string HashedPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int  Status { get; set; }

    }
}
