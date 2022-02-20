using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Serialization.csv
{
    public abstract class CsvSerializable
    {
        private Dictionary<string, CsvSerializerData> GetProperties()
        {
            Dictionary<string, CsvSerializerData> _props = new();
            PropertyInfo[] props = this.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var attr = prop.GetCustomAttributes<CsvColumnAttribute>(true).FirstOrDefault();
                if (attr != null)
                {
                    _props.Add(prop.Name, new CsvSerializerData() { Info = attr, Property = prop });
                }
            }
            return _props
                .OrderBy(o => o.Value.Info.Position)
                .ToDictionary(k => k.Key, v => v.Value);
        }

        public string ToCsvHead(string separator = ",")
        {
            if (string.IsNullOrWhiteSpace(separator))
            {
                throw new ArgumentException(nameof(separator));
            }
            return string.Join(separator, GetProperties().Select(o => $"\"{(string.IsNullOrWhiteSpace(o.Value.Info.Name) ? o.Key : o.Value.Info.Name).ToString().Replace("\"", "\"\"")}\""));
        }

        public string ToCsvLine(string separator = ",")
        {
            if (string.IsNullOrWhiteSpace(separator))
            {
                throw new ArgumentException(nameof(separator));
            }
            return string.Join(separator, GetProperties().Select(o => $"\"{(o.Value.Property.GetValue(this) ?? "").ToString().Replace("\"", "\"\"")}\""));
        }
    }
}
