using IndacoProject.Corso.Data.Serialization.csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Models
{
    public class MessageCsvModel: CsvSerializable
    {
        public byte[] RowVersion { get; set; }
        public string Id { get; set; }

        [CsvColumn("Nome Utente", 0)]
        public string Name { get; set; }

        [CsvColumn("Mail", 1)]
        public string Email { get; set; }

        [CsvColumn("Oggetto", 2)]
        public string Subject { get; set; }

        [CsvColumn("Testo", 3)]
        public string Body { get; set; }

        public bool Sent { get; set; }
        public int Attempts { get; set; }
    }
}
