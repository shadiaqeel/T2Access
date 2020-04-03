using System;

namespace T2Access.DAL
{
    public class BaseEntity : IEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
