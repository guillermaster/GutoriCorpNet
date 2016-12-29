using GutoriCorp.Models.GeneralViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GutoriCorpNet.Common;


namespace GutoriCorp.Data.Operations
{
    public class GeneralCatalogData
    {
        private readonly ApplicationDbContext _context;

        public GeneralCatalogData(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<GeneralCatalogValue> GetCatalogValues(Enums.GeneralCatalog gralCatalogId)
        {
            var catValues = _context.GeneralCatalogValues.Where(cat => cat.catalog_id == (int)gralCatalogId).ToList();
            return catValues;
        }
    }
}
