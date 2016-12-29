using GutoriCorp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Operations
{
    public class DriverData
    {
        private readonly ApplicationDbContext _context;

        public DriverData(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Driver> GetDrivers()
        {
            return _context.Driver.ToList();
        }
    }
}
