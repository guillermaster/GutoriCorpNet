using GutoriCorp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Operations
{
    public class VehicleMakeData
    {
        private readonly ApplicationDbContext _context;

        public VehicleMakeData(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<VehicleMake> GetMakes()
        {
            return _context.VehicleMake.OrderBy(m => m.name).ToList();
        }

        public List<VehicleMakeModel> GetMakeModels(short makeId)
        {
            return _context.VehicleMakeModel.Where(m => m.make_id == makeId).OrderBy(m => m.name).ToList();
        }
    }
}
