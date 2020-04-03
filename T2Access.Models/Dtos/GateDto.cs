using System;

namespace T2Access.Models.Dtos
{
    public class GateDto
    {

        public Guid Id { get; set; }


        public string UserName { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public int? Status { get; set; }
    }

    public class CheckedGateDto : GateDto
    {

        public bool Checked { get; set; }


    }

}
