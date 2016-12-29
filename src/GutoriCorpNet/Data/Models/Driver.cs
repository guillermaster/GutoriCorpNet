using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    [Table("Driver")]
    public partial class Driver
    {
        public Driver()
        {
            //Contracts = new HashSet<Contract>();
            //DriverLicenses = new HashSet<DriverLicense>();
        }

        public short id { get; set; }

        [Required]
        [StringLength(100)]
        public string first_name { get; set; }

        [Required]
        [StringLength(100)]
        public string last_name { get; set; }

        [Required]
        [StringLength(20)]
        public string phone { get; set; }

        [StringLength(150)]
        public string address { get; set; }

        [Required]
        [StringLength(20)]
        public string ssn { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        //public virtual ICollection<Contract> Contracts { get; set; }

        //public virtual SystemUser SystemUser { get; set; }

        //public virtual ICollection<DriverLicense> DriverLicenses { get; set; }

        //public virtual SystemUser SystemUser1 { get; set; }

        public override string ToString()
        {
            return first_name + " " + last_name;
        }
    }
}
