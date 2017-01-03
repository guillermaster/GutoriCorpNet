using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.BusinessViewModels
{
    public class OwnerViewModel
    {
        public short id { get; set; }

        [StringLength(100)]
        public string first_name { get; set; }

        [StringLength(100)]
        public string last_name { get; set; }

        [StringLength(150)]
        public string business_name { get; set; }

        [Required]
        [StringLength(20)]
        public string phone { get; set; }

        [StringLength(150)]
        public string address { get; set; }

        [StringLength(75)]
        public string address2 { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }
    }
}
