using System;

namespace T2Access.DAL
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Status { get; set; }

    }
}
