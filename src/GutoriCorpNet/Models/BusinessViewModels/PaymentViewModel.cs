using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.BusinessViewModels
{
    public class PaymentViewModel
    {
        public long id { get; set; }

        public long contract_id { get; set; }

        public short period { get; set; }

        public DateTime payment_date { get; set; }

        public bool late { get; set; }

        public bool tickets { get; set; }
        
        public decimal rental_fee { get; set; }

        public decimal late_fee { get; set; }

        public decimal tickets_fee { get; set; }

        public decimal total_due_amount { get; set; }

        public decimal total_paid_amount { get; set; }

        public decimal balance { get; set; }

        public decimal credit { get; set; }

        public decimal previous_balance { get; set; }

        public decimal previous_credit { get; set; }

        public short status_id { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        /*** display-only properties ***/

        [Display(Name = "Lessor")]
        public string lessor { get; set; }

        [Display(Name = "Lessee")]
        public string lessee { get; set; }

        [Display(Name = "Frecuency")]
        public string frequency { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "TLC Plate")]
        public string tlc_plate { get; set; }

        [Display(Name = "VIN Code")]
        public string vin_code { get; set; }

        [Display(Name = "Due Date")]
        public DateTime due_date { get; set; }

        public short frequency_id { get; set; }

        public short due_day { get; set; }

        public int vehicle_id { get; set; }
    }
}
