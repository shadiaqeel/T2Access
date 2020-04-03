using System;

namespace T2Access.Models
{
    public class TransactionModel : BaseModel
    {
        public decimal Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GateId { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }

    }
}
