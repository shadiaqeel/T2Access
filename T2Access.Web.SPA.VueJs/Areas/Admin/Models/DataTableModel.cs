using T2Access.Models.Resources;

namespace T2Access.Web.SPA.VueJs.Areas.Admin.Models
{
    public class DTParameters
    {
        public int Start  { get; set; }
        public int Length  { get; set; }

    }

    public class DTUserParameters : DTParameters
    {

        public UserFilter Filter { get; set; }

    }
    
    public class DTGateParameters : DTParameters
    {

        public GateFilter Filter { get; set; }

    }



    
     public class UserFilter 
     {

         public string Username { get; set; }
         public string Firstname { get; set; }
         public string Lastname { get; set; }
         public int? Status { get; set; }
     }

     public class GateFilter 
     {

         public string Username { get; set; }
         public string Namear { get; set; }
         public string Nameen { get; set; }
         public int? Status { get; set; }
     }
     





}