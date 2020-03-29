using System;
using System.ComponentModel.DataAnnotations;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public interface IAuthModel
    {
        Guid Id { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

    }
}
