using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Core
{
    public interface ICipherService
    {
        string EncryptString(string text, string keyString);
        string DecryptString(string cipherText, string keyString);
    }
}
