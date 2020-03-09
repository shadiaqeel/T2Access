using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{
    public class UserGate : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GateId { get; set; }
    }
}
