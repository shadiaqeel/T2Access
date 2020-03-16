using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Security.Tokenization.Models
{
    public interface IJWTClaimsModel
    {
         string Username { get; set; }
         string Role { get; set; }
    }
}
