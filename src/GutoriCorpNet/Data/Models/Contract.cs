using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutoriCorp.Data.Models
{
    [Table("Contract")]
    public partial class Contract
    {
        public long id { get; set; }

        public short lessor_id { get; set; }

        public short lessee_id { get; set; }

        public short contract_type_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime contract_date { get; set; }

        public short? frequency_id { get; set; }

        public decimal? rental_fee { get; set; }

        public short? late_fee_type_id { get; set; }

        public decimal? late_fee { get; set; }

        public decimal? thirdparty_fee { get; set; }

        public decimal? accident_penalty_fee { get; set; }

        public short status_id { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        public int? vehicle_id { get; set; }

        public short? due_day { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue { get; set; }

        //public virtual SystemUser SystemUser { get; set; }

        //public virtual Driver Driver { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue1 { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue2 { get; set; }

        //public virtual SystemUser SystemUser1 { get; set; }

        //public virtual Owner Owner { get; set; }

        //public virtual GeneralCatalogValue GeneralCatalogValue3 { get; set; }

    }
}
