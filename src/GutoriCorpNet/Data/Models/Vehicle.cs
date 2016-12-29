using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    [Table("Vehicle")]
    public partial class Vehicle
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string vin_code { get; set; }

        [Required]
        [StringLength(4)]
        public string year { get; set; }

        public short make_id { get; set; }

        public short model_id { get; set; }

        public short body_hull_id { get; set; }

        [Required]
        [StringLength(10)]
        public string tlc_plate { get; set; }

        [StringLength(10)]
        public string document_num { get; set; }

        public short color_id { get; set; }

        public decimal? wt_sts_lgth { get; set; }

        public byte seats { get; set; }

        public short fuel_id { get; set; }

        public decimal cyl_prop { get; set; }

        [Column("new")]
        public bool is_new { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date_issued { get; set; }

        public decimal reading_miles { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        public short owner_id { get; set; }

        public short? driver_id { get; set; }

        public short status_id { get; set; }

        [Required]
        public short type_id { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue1 { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue2 { get; set; }

        //public virtual SystemUser SystemUser { get; set; }

        //public virtual SystemUser SystemUser1 { get; set; }

        //public virtual VehicleMake VehicleMake { get; set; }

        //public virtual VehicleMakeModel VehicleMakeModel { get; set; }
    }
}
