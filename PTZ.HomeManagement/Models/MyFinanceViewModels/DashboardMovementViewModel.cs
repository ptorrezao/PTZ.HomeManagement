﻿using System;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class DashboardMovementViewModel: DashboardAccountViewModel
    {
        public DashboardMovementViewModel()
        {
        }

        public DateTime Day { get;  set; }
    }
}