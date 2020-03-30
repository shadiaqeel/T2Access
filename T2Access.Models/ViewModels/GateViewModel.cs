using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using T2Access.Models.Resources;



namespace T2Access.Models
{

    //[JsonConverter(typeof(StringEnumConverter))]
    public enum GateStatus
    {
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Active), GroupName = "blue")]
        Active = 0,
        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Block), GroupName = "red")]
        Block = 1
    }

    public class GateViewModel  
    {
        public Guid Id { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameAr))]
        public string NameAr { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.NameEn))]
        public string NameEn { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = nameof(ModelResource.Status))]
        public GateStatus? Status { get; set; }
        public bool Checked { get; set; }
    }
}