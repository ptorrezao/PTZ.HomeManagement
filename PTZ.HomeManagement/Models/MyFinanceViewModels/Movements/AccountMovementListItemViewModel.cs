﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountMovementListItemViewModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("MovementDate")]
        public DateTime MovementDate { get; set; }

        [DisplayName("ValueDate")]
        public DateTime ValueDate { get; set; }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [DisplayName("TotalBalanceAfterMovement")]
        public decimal TotalBalanceAfterMovement { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Categories")]
        public List<CategoryListItemViewModel> Categories { get; set; }
    }
}