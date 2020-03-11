using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{
    public class TransactionModel: BaseModel
    {
        public decimal Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GateId { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }

    }
}
