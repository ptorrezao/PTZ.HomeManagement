using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.Services
{
    public interface ICoreService
    {
        List<string> GetLastNMessages(int qtdOfMessages, string filename = null);
    }
}
