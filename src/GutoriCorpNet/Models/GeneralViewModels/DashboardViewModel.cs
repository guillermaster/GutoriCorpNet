using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.GeneralViewModels
{
    public class DashboardViewModel
    {
        public int TotalActiveContracts { get; set; }
        public int TotalActiveVehicles { get; set; }
        public int TotalFreeVehicles { get; set; }
        public decimal TotalIncomeLastmonth { get; set; }
        public List<DashboardSerieViewModel> PaymentsThisWeek { get; set; }
        public List<DashboardSerieViewModel> PaymentsThisMonth { get; set; }
    }
}
