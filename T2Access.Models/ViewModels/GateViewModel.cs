using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using T2Access.Models.Resources;

namespace T2Access.Models
{
    public enum GateStatus
    {
        Regular = 0,
        Block = 1
    }

    public class GateViewModel
    {
        public Guid Id { get; set; }


        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.NameAr))]
        public string NameAr { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.NameEn))]
        public string NameEn { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Status))]
        public GateStatus Status { get; set; }
    }
}