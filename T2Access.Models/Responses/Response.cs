using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.Models
{

    interface IResponseBase
    { }

    public class ResponseBase
    { }


    public class ListResponse<T> : ResponseBase
    {
        public IList<T> ResponseList { get; set; }
        public int? totalEntities { get; set; }

    }

    public class GateListResponse : ListResponse<IGateModel>
    { }

    public class UserListResponse : ListResponse<IUserModel>
    { }

}
