using GutoriCorp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Operations
{
    public class OwnerData
    {
        private readonly ApplicationDbContext _context;

        public OwnerData(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Owner> GetOwners()
        {
            return _context.Owner.ToList();
        }
    }
}
