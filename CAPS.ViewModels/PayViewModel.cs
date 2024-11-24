﻿namespace CAPS.ViewModels
{
    public class PayViewModel
    {
        public int LoanId { get; set; }
        public decimal Amount { get; set; }

        public decimal LoanInitialAmount { get; set; }
        public string AppUserId { get; set; }
    }
}