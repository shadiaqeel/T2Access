using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.DAL
{
    public class BaseEntity : IEntity
    {
        public DateTime CreatedDate { get ; set ; } = DateTime.Now;
    }
}
