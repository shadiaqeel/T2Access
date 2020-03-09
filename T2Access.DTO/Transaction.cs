using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{
    public class Transaction: BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string GateId { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }

    }
}
