﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}