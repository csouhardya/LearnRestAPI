using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IPasswordHelper
    {
        string Hash(string password);
        bool Verify(string input, string hash);
    }
}
