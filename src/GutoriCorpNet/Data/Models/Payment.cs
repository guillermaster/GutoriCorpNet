using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    [Table("Payment")]
    public class Payment
    {
        public long id { get; set; }

        public long contract_id { get; set; }

        public short period { get; set; }

        public DateTime payment_date { get; set; }

        public DateTime due_date { get; set; }

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
    }
}
