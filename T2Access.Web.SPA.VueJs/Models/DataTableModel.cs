using System.Collections.Generic;

namespace T2Access.Web.SPA.VueJs.Models
{
    public class DTResult<T>
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public IEnumerable<T> Data { get; set; }
    }

    public abstract class DTRow
    {
        public virtual string DT_RowId => null;

        public virtual string DT_RowClass => null;

        public virtual object DT_RowData => null;
    }

    public class DTParameters
    {
        public int Draw { get; set; }

        public DTColumn[] Columns { get; set; }

        public DTOrder[] Order { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public DTSearch Search { get; set; }

        public string SortOrder => Columns != null && Order != null && Order.Length > 0
                    ? (Columns[Order[0].Column].Data + (Order[0].Dir == DTOrderDir.DESC ? " " + Order[0].Dir : string.Empty))
                    : null;

        public int SearchOption { get; set; }

    }

    public class DTColumn
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public DTSearch Search { get; set; }
    }

    public class DTOrder
    {
        public int Column { get; set; }

        public DTOrderDir Dir { get; set; }
    }

    public enum DTOrderDir
    {
        ASC,
        DESC
    }

    public class DTSearch
    {
        public string Value { get; set; }

        public bool Regex { get; set; }
    }

}
