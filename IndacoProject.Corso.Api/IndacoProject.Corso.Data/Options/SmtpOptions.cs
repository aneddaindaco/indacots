using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Options
{
    public class SmtpOptions
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SSL { get; set; }
        public int RetryAttempt { get; set; }
        public int CheckTimeout { get; set; }
    }
}
