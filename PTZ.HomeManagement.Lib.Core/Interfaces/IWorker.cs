using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ.HomeManagement.Interfaces
{
    public interface IWorker
    {
        List<IWorkerJob> GetJobs();
        string GetName();
    }
}
