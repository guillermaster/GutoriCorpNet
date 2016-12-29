using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.BusinessViewModels
{
    public class ContractVehicleViewModel
    {
        [Required]
        [Display(Name = "Contract ID")]
        public long contract_id { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public int vehicle_id { get; set; }

        [Required]
        [Display(Name = "Lessee")]
        public short lessee_id { get; set; }
        
        [Display(Name = "Lessor")]
        public string LessorName { get; set; }

        [Display(Name = "Lessee")]
        public string LesseeName { get; set; }

        public IEnumerable<SelectListItem> AvailableVehicles { get; set; }
    }
}
