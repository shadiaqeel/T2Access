using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{
    public class UserDto
    {
    
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string password { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Status { get; set; } = 0;

    }


}
