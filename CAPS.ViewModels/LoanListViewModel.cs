using CAPS.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.ViewModels
{
    public class LoanListViewModel
    {
        public List<Loan> ApprovedLoans { get; set; } = new List<Loan>();
        public List<Loan> DeclinedLoans { get; set; } = new List<Loan>();
    }
}
