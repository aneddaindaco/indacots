using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Serialization.csv
{
    internal class CsvSerializerData
    {
        internal CsvColumnAttribute Info { get; set; }
        internal PropertyInfo Property { get; set; }
    }
}
