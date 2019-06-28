using System;
using System.Collections.Generic;
using System.Text;

namespace Zapdate.Core.Interfaces.Services
{
    public interface IEncrypter
    {
        string EncryptString(string source, string password);
        string DecryptString(string source, string password);

        byte[] Encrypt(byte[] source, byte[] key);
    }
}
