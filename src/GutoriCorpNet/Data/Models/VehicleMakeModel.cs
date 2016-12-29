using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    [Table("VehicleMakeModel")]
    public partial class VehicleMakeModel
    {
        public VehicleMakeModel()
        {
            //Vehicles = new HashSet<Vehicle>();
        }

        public short id { get; set; }

        public short make_id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        //public virtual ICollection<Vehicle> Vehicles { get; set; }

        //public virtual VehicleMake VehicleMake { get; set; }
    }
}
