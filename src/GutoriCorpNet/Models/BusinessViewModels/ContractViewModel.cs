using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutoriCorp.Models.BusinessViewModels
{
    public partial class ContractViewModel
    {
        [Display(Name = "ID")]
        public long id { get; set; }

        [Required]
        [Display(Name = "Lessor")]
        public short lessor_id { get; set; }

        [Display(Name = "Lessor")]
        public string lessor { get; set; }

        [Required]
        [Display(Name = "Lessee")]
        public short lessee_id { get; set; }

        [Display(Name = "Lessee")]
        public string lessee { get; set; }

        [Required]
        [Display(Name="Type")]
        public short contract_type_id { get; set; }
        
        [Display(Name = "Type")]
        public string contract_type { get; set; }

        [Required]
        [Display(Name = "Contract Date")]
        [Column(TypeName = "date")]
        public DateTime contract_date { get; set; }
        
        [Display(Name = "Frecuency")]
        public short? frequency_id { get; set; }

        [Display(Name = "Frecuency")]
        public string frequency { get; set; }

        [Display(Name = "Rental Fee")]
        public decimal? rental_fee { get; set; }
        
        [Display(Name = "Late Fee Type")]
        public short? late_fee_type_id { get; set; }

        [Display(Name = "Late Fee Type")]
        public string late_fee_type { get; set; }

        [Display(Name = "Late Fee")]
        public decimal? late_fee { get; set; }
        
        [Display(Name = "Thirdparty Fee")]
        public decimal? thirdparty_fee { get; set; }

        [Display(Name = "Addicent Penalty Fee")]
        public decimal? accident_penalty_fee { get; set; }

        [Display(Name = "Status")]
        public short status_id { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        public int? vehicle_id { get; set; }

        [Display(Name = "Due Day")]
        public short? due_day { get; set; }

        public string due_day_desc { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue { get; set; }

        //public virtual SystemUser SystemUser { get; set; }

        //public virtual Driver Driver { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue1 { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue2 { get; set; }

        //public virtual SystemUser SystemUser1 { get; set; }

        //public virtual Owner Owner { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue3 { get; set; }

        // Properties used to fill dropdownlists
        public IEnumerable<SelectListItem> ContractTypes { get; set; }
        public IEnumerable<SelectListItem> ContractFrequencies { get; set; }
        public IEnumerable<SelectListItem> ContractLateFeeTypes { get; set; }
        public IEnumerable<SelectListItem> Owners { get; set; }
        public IEnumerable<SelectListItem> Drivers { get; set; }
        //public List<GeneralViewModels.GeneralCatalogValue> Frecuencies { get; set; }
        //public List<GeneralViewModels.GeneralCatalogValue> LateFeesTypes { get; set; }
    }
}
