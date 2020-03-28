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


    public class ResponseFilteredList<T> : ResponseBase
    {
        public List<T> ResponseList { get; set; }
        public int? totalEntities { get; set; }

    }

    public class ResponseFilteredGateList : ResponseFilteredList<GateModel>
    { }

    public class ResponseFilteredUserList : ResponseFilteredList<UserModel>
    { }

}
