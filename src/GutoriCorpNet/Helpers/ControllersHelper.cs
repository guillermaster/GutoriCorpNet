using GutoriCorp.Data;
using GutoriCorp.Data.Operations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Helpers
{
    public class ControllersHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly string selectMsg = " - Select -";

        public ControllersHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetGeneralCatalogValues(GutoriCorpNet.Common.Enums.GeneralCatalog catalog, bool addSelectEmpty = true)
        {
            var selectList = new List<SelectListItem>();

            if (addSelectEmpty)
            {
                selectList.Add(new SelectListItem { Value = string.Empty, Text = selectMsg });
            }

            var gralCatalogDataOp = new GeneralCatalogData(_context);
            var contractTypes = gralCatalogDataOp.GetCatalogValues(catalog);

            foreach (var contractType in contractTypes)
            {
                selectList.Add(new SelectListItem
                {
                    Value = contractType.id.ToString(),
                    Text = contractType.title
                });
            }

            return selectList;
        }

        public IEnumerable<SelectListItem> GetAllOwners(bool addSelectEmpty = true)
        {
            var selectList = new List<SelectListItem>();

            if (addSelectEmpty)
            {
                selectList.Add(new SelectListItem { Value = string.Empty, Text = selectMsg });
            }

            var ownerDataOp = new OwnerData(_context);
            var owners = ownerDataOp.GetOwners();

            foreach (var owner in owners)
            {
                selectList.Add(new SelectListItem
                {
                    Value = owner.id.ToString(),
                    Text = owner.first_name + " " + owner.last_name
                });
            }

            return selectList;
        }
    }
}
