using System;

namespace T2Access.DAL
{
    public class UserGate : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GateId { get; set; }
    }
}
