using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.BusinessViewModels
{
    public class VehicleViewModel
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "VIN Code")]
        public string vin_code { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "Year")]
        public string year { get; set; }

        [Required]
        [Display(Name = "Make")]
        public short make_id { get; set; }

        [Required]
        [Display(Name = "Model")]
        public short model_id { get; set; }

        [Required]
        [Display(Name = "Body Hull")]
        public short body_hull_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "TLC Plate")]
        public string tlc_plate { get; set; }

        [StringLength(10)]
        [Display(Name = "Document Number")]
        public string document_num { get; set; }

        [Required]
        [Display(Name = "Color")]
        public short color_id { get; set; }

        [Required]
        [Display(Name = "Weight STS Length")]
        public decimal? wt_sts_lgth { get; set; }

        [Required]
        [Display(Name = "Number of Seats")]
        public byte seats { get; set; }

        [Required]
        [Display(Name = "Fuel")]
        public short fuel_id { get; set; }

        [Required]
        [Display(Name = "Cyl")]
        public decimal cyl_prop { get; set; }

        [Required]
        [Display(Name = "Is New?")]
        public bool is_new { get; set; }
        
        [Column(TypeName = "date")]
        [Display(Name = "Date Issued")]
        public DateTime? date_issued { get; set; }

        [Required]
        [Display(Name = "Reading Miles")]
        public decimal reading_miles { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        [Required]
        [Display(Name = "Owner")]
        public short owner_id { get; set; }

        [Display(Name = "Driver")]
        public short? driver_id { get; set; }

        [Display(Name = "Status")]
        public short status_id { get; set; }

        [Required]
        [Display(Name = "Type")]
        public short type_id { get; set; }

        public string owner { get; set; }

        public string make { get; set; }

        public string model { get; set; }

        public string body_hull { get; set; }

        public string color { get; set; }

        public string fuel { get; set; }

        public string status { get; set; }

        public string type { get; set; }

        public string driver { get; set; }

        public string created_by_name { get; set; }

        public string modified_by_name { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }
        public IEnumerable<SelectListItem> Makes { get; set; }
        public IEnumerable<SelectListItem> Models { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> BodyHulls { get; set; }
        public IEnumerable<SelectListItem> Colors { get; set; }
        public IEnumerable<SelectListItem> Fuels { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public override string ToString()
        {
            return make + " " + model + " " + year + " - " + tlc_plate;
        }
    }
}
