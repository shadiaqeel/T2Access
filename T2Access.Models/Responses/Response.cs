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

    public interface IListResponse<T>
    {
        IEnumerable<T> ResponseList { get; set; }
        int? TotalEntities { get; set; }
    }




    //==============================================================================

    public class ListResponse<T> : IListResponse<T>
    {
        public IEnumerable<T> ResponseList { get; set; }
        public int? TotalEntities { get; set; }

    }

    public class GateListResponse : ListResponse<GateDto>
    { }

    public class UserListResponse : ListResponse<UserDto>
    { }

}
