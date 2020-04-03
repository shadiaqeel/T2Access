using System.Collections.Generic;

using T2Access.Models.Dtos;

namespace T2Access.Models
{




    public interface IServiceResponce<T>
    {

        T Data { get; set; }

        bool Success { get; set; }

        string ErrorMessage { get; set; }


    }


    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; } = true;

        public string ErrorMessage { get; set; } = string.Empty;
    }




    //==============================================================================

    public class ListResponse<T>
    {
        public IEnumerable<T> ResponseList { get; set; }
        public int? totalEntities { get; set; }

    }

    public class GateListResponse : ListResponse<GateDto>
    { }

    public class UserListResponse : ListResponse<UserDto>
    { }

}
