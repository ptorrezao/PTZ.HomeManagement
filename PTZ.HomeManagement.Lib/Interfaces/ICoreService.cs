using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.Interfaces
{
    public interface ICoreService
    {
        List<string> GetLastNMessages(int qtdOfMessages, string filename = null);
    }
}
