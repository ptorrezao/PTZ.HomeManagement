using Microsoft.AspNetCore.Http;
using PTZ.HomeManagement.Models;
using PTZ.HomeManagement.MyFinance.Models;
using PTZ.HomeManagement.MyFinance.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using PTZ.HomeManagement.MyFinance.Data;
using PTZ.HomeManagement.Core.Data;

namespace PTZ.HomeManagement.MyFinance
{
    public partial class MyFinanceService : IMyFinanceService
    {
        private readonly IMyFinanceRepository myFinanceRepo;
        private readonly IApplicationRepository appRepo;

        private BankAccountMovementImportFactory factory;

        public MyFinanceService(IMyFinanceRepository repo,
            IApplicationRepository dbContext)
        {
            this.myFinanceRepo = repo;
            this.appRepo = dbContext;
        }

    }
}
