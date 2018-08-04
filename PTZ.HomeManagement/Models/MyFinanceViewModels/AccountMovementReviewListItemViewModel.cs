using System;
using System.ComponentModel;

namespace PTZ.HomeManagement.Models.MyFinanceViewModels
{
    public class AccountMovementReviewListItemViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("MovementDate")]
        public DateTime MovementDate { get; set; }

        [DisplayName("ValueDate")]
        public DateTime ValueDate { get; set; }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}