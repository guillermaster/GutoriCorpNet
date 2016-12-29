using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutoriCorp.Models.GeneralViewModels
{
    [Table("GeneralCatalogValue")]
    public partial class GeneralCatalogValue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short id { get; set; }

        public short catalog_id { get; set; }

        [Required]
        [StringLength(50)]
        public string title { get; set; }
    }
}
