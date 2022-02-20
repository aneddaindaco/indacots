using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Serialization.csv
{
    public class CsvColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public CsvColumnAttribute()
        {
        }
        public CsvColumnAttribute(string name) : this()
        {
            Name = name;
        }
        public CsvColumnAttribute(string name, int position) : this(name)
        {
            Position = position;
        }
    }
}
