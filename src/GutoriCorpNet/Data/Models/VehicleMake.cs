using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    [Table("VehicleMake")]
    public partial class VehicleMake
    {
        public VehicleMake()
        {
            //Vehicles = new HashSet<Vehicle>();
            //VehicleMakeModels = new HashSet<VehicleMakeModel>();
        }

        public short id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        //public virtual ICollection<Vehicle> Vehicles { get; set; }

        //public virtual ICollection<VehicleMakeModel> VehicleMakeModels { get; set; }
    }
}
