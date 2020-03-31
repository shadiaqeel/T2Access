using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models.Dtos;

namespace T2Access.Models
{




    public interface IServiceResponce<T>
    {

         T Data { get; set; }

         bool Success { get; set; } 

         IList<string> Messages { get; set; } 


    }


    public class ServiceResponse<T>
    {
        public T Data { get; set; } 

        public bool Success { get; set; } = true;

        public IList<string> Messages { get; set; } = new List<string>();
    }




    //==============================================================================

    public class ListResponse<T> 
    {
        public IList<T> ResponseList { get; set; }
        public int? totalEntities { get; set; }

    }

    public class GateListResponse : ListResponse<GateDto>
    { }

    public class UserListResponse : ListResponse<UserDto>
    { }

}
