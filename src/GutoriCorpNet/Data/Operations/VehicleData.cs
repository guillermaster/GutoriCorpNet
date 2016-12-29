using GutoriCorp.Data.Models;
using GutoriCorp.Models.BusinessViewModels;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Operations
{
    public class VehicleData
    {
        private readonly ApplicationDbContext _context;

        public VehicleData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(VehicleViewModel contract)
        {
            _context.Add(GetEntity(contract));
            await _context.SaveChangesAsync();
        }

        public async Task Update(VehicleViewModel vehicle)
        {
            var vehicleDb = GetEntity(vehicle);
            vehicleDb.id = vehicle.id;
            _context.Update(vehicleDb);
            await _context.SaveChangesAsync();
        }

        public void SetDriver(int vehicleId, short driverId, short userId)
        {
            var vehicle = new Vehicle
            {
                id = vehicleId,
                driver_id = driverId,
                modified_by = userId,
                modified_on = DateTime.Now
            };

            _context.Attach(vehicle);
            _context.Entry(vehicle).Property(c => c.driver_id).IsModified = true;
            _context.Entry(vehicle).Property(c => c.modified_by).IsModified = true;
            _context.Entry(vehicle).Property(c => c.modified_on).IsModified = true;
            _context.SaveChanges();
        }

        public VehicleViewModel Get(long? id, bool elementaryData = false)
        {
            if (id == null)
            {
                throw new KeyNotFoundException();
            }

            var vehiclesQry = elementaryData ? QueryElementaryData() : QueryAllData();

            vehiclesQry = vehiclesQry.Where(v => v.id == id);

            var vehicle = vehiclesQry.SingleOrDefault(m => m.id == id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException();
            }
            return vehicle;
        }

        public async Task<List<VehicleViewModel>> GetAll(bool elementaryData = false)
        {
            var vehiclesQry = elementaryData ? QueryElementaryData() : QueryAllData();
            return await vehiclesQry.ToListAsync();
        }

        /// <summary>
        /// Returns a list of all vehicles without driver
        /// </summary>
        /// <param name="elementaryData">True to load only the ID and basic data as make-model-year; false to load all vehicle data</param>
        /// <returns></returns>
        public List<VehicleViewModel> GetAllFree(bool elementaryData = false)
        {
            var vehiclesQry = QueryElementaryData();
            vehiclesQry = vehiclesQry.Where(v => v.driver_id == null);
            return vehiclesQry.ToList();
        }

        private IQueryable<VehicleViewModel> QueryAllData()
        {
            var vehiclesQry = from veh in _context.Vehicle
                               join own in _context.Owner on veh.owner_id equals own.id
                               join driv in _context.Driver on veh.driver_id equals driv.id into dveh
                               from drivVeh in dveh.DefaultIfEmpty()
                               join make in _context.VehicleMake on veh.make_id equals make.id
                               join model in _context.VehicleMakeModel on veh.model_id equals model.id
                               join hull in _context.GeneralCatalogValues on veh.body_hull_id equals hull.id
                               join color in _context.GeneralCatalogValues on veh.color_id equals color.id
                               join fuel in _context.GeneralCatalogValues on veh.fuel_id equals fuel.id
                               join stat in _context.GeneralCatalogValues on veh.status_id equals stat.id
                               join type in _context.GeneralCatalogValues on veh.type_id equals type.id
                               join cuser in _context.SystemUser on veh.created_by equals cuser.id
                               join muser in _context.SystemUser on veh.modified_by equals muser.id
                               select new VehicleViewModel
                               {
                                   id = veh.id,
                                   owner_id = veh.owner_id,
                                   owner = own.ToString(),
                                   driver_id = veh.driver_id,
                                   driver = drivVeh != null ? drivVeh.ToString() : string.Empty,
                                   vin_code = veh.vin_code,
                                   year = veh.year,
                                   make_id = veh.make_id,
                                   make = make.name,
                                   model_id = veh.model_id,
                                   model = model.name,
                                   body_hull_id = veh.body_hull_id,
                                   body_hull = hull.title,
                                   tlc_plate = veh.tlc_plate,
                                   document_num = veh.document_num,
                                   color_id = veh.color_id,
                                   color = color.title,
                                   seats = veh.seats,
                                   wt_sts_lgth = veh.wt_sts_lgth,
                                   fuel_id = veh.fuel_id,
                                   fuel = fuel.title,
                                   cyl_prop = veh.cyl_prop,
                                   is_new = veh.is_new,
                                   date_issued = veh.date_issued,
                                   reading_miles = veh.reading_miles,
                                   type_id = veh.type_id,
                                   type = type.title,
                                   status_id = veh.status_id,
                                   status = stat.title,
                                   created_on = veh.created_on,
                                   created_by = veh.created_by,
                                   created_by_name = cuser.ToString(),
                                   modified_on = veh.modified_on,
                                   modified_by = veh.modified_by,
                                   modified_by_name = muser.ToString()
                               };
            return vehiclesQry;
        }

        private IQueryable<VehicleViewModel> QueryElementaryData()
        {
            var vehiclesQry = from veh in _context.Vehicle
                              join make in _context.VehicleMake on veh.make_id equals make.id
                              join model in _context.VehicleMakeModel on veh.model_id equals model.id
                              select new VehicleViewModel
                              {
                                  id = veh.id,
                                  year = veh.year,
                                  make_id = veh.make_id,
                                  make = make.name,
                                  model_id = veh.model_id,
                                  model = model.name,
                                  tlc_plate = veh.tlc_plate,
                                  driver_id = veh.driver_id
                              };
            return vehiclesQry;
        }

        private Vehicle GetEntity(VehicleViewModel vehicleVm)
        {
            var vehicle = new Vehicle
            {
                vin_code = vehicleVm.vin_code,
                year = vehicleVm.year,
                make_id = vehicleVm.make_id,
                model_id = vehicleVm.model_id,
                body_hull_id = vehicleVm.body_hull_id,
                tlc_plate = vehicleVm.tlc_plate,
                document_num = vehicleVm.document_num,
                color_id = vehicleVm.color_id,
                wt_sts_lgth = vehicleVm.wt_sts_lgth,
                seats = vehicleVm.seats,
                fuel_id = vehicleVm.fuel_id,
                cyl_prop = vehicleVm.cyl_prop,
                is_new = vehicleVm.is_new,
                date_issued = vehicleVm.date_issued,
                reading_miles = vehicleVm.reading_miles,
                owner_id = vehicleVm.owner_id,
                driver_id = vehicleVm.driver_id,
                created_on = DateTime.Now,
                created_by = vehicleVm.created_by,
                modified_on = DateTime.Now,
                modified_by = vehicleVm.modified_by,
                type_id = vehicleVm.type_id,
                status_id = vehicleVm.status_id
            };
            return vehicle;
        }
    }
}
