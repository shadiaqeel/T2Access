using System;

namespace T2Access.Models
{
    public interface IAuthModel
    {
        Guid Id { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

    }
}
