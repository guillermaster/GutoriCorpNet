using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.BusinessViewModels
{
    public class TicketViewModel
    {
        public long id { get; set; }

        public int vehicle_id { get; set; }

        [Required]
        [StringLength(50)]
        public string vin_code { get; set; }

        [Required]
        [StringLength(10)]
        public string tlc_plate { get; set; }

        [Required]
        [StringLength(50)]
        public string ticket_num { get; set; }

        [Required]
        [StringLength(120)]
        public string description { get; set; }

        [StringLength(120)]
        public string occurrence_place { get; set; }

        [Column(TypeName = "date")]
        public DateTime ticket_date { get; set; }

        public decimal fine_amount { get; set; }

        public byte[] ticket_file { get; set; }

        public string ticket_file_type { get; set; }

        public string ticket_file_name { get; set; }

        public bool paid { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        public short status_id { get; set; }

        /** Display only properties **/
        
        public VehicleViewModel vehicle { get; set; }

        public string vehicle_name { get { return vehicle != null ? vehicle.ToString() : string.Empty; } }

        public string refer_url { get; set; }

        public string status { get; set; }
    }
}
