using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    [Table("DriverLicense")]
    public class DriverLicense
    {
        public short id { get; set; }

        public short driver_id { get; set; }

        public short license_type_id { get; set; }

        [Required]
        [StringLength(20)]
        public string number { get; set; }

        [Required]
        public DateTime expire_date { get; set; }
    }
}
