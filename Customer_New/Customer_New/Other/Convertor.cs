using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Other
{
    public static class Convertor
    {
        public static IEnumerable<DataRow> AsEnumerable(this DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }
    }
}
