using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Core
{
    public interface INotify
    {
        Task DisplayMessage(string message);
        Task NotifyMail(string message);
    }
}
