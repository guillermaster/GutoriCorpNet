using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using GutoriCorp.Data;
using GutoriCorp.Data.Models;
using GutoriCorp.Data.Operations;
using GutoriCorp.Models.BusinessViewModels;
using GutoriCorpNet.Common;
using GutoriCorp.Helpers;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GutoriCorp.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicleDataOp = new VehicleData(_context);
            return View(await vehicleDataOp.GetAll());
        }

        // GET: Vehicles/Details/5
        public IActionResult Details(int? id)
        {
            try
            {
                var vehicleDataOp = new VehicleData(_context);
                var vehicle = vehicleDataOp.Get(id);
                return View(vehicle);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            var model = new VehicleViewModel();
            PopulateVehicleOptionLists(model);

            return View(model);
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("id,_new,body_hull_id,color_id,created_by,created_on,cyl_prop,date_issued,document_num,fuel_id,make_id,model_id,modified_by,modified_on,reading_miles,seats,tlc_plate,type_title,vin_code,wt_sts_lgth,year")] Vehicle vehicle)
        public async Task<IActionResult> Create(VehicleViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.modified_by = 1;
                vehicle.created_by = 1;
                vehicle.status_id = (short)Enums.GeneralStatus.Active;

                var vehicleDataOp = new VehicleData(_context);
                await vehicleDataOp.Add(vehicle);
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public IActionResult Edit(int? id)
        {
            try
            {
                var vehicleDataOp = new VehicleData(_context);
                var vehicle = vehicleDataOp.Get(id);
                PopulateVehicleOptionLists(vehicle);
                return View(vehicle);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleViewModel vehicle)
        {
            if (id != vehicle.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehicleDataOp = new VehicleData(_context);
                    vehicle.modified_by = 1;
                    vehicle.modified_on = DateTime.Now;
                    await vehicleDataOp.Update(vehicle);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.SingleOrDefaultAsync(m => m.id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.SingleOrDefaultAsync(m => m.id == id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

//        [HttpPost]
        public JsonResult Models(short year)
        {
            //if (year == 2011)
            //{
            //    return Json(
            //        Enumerable.Range(1, 3).Select(x => new { value = x, text = x }),
            //        JsonRequestBehavior.AllowGet
            //    );
            //}
            return Json(
                GetMakeModels(year).Select(x => new { value = x.Value, text = x.Text })//,
                //JsonRequestBehavior.AllowGet
            );
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.id == id);
        }

        private void PopulateVehicleOptionLists(VehicleViewModel vehicleVm)
        {
            var helper = new ControllersHelper(_context);
            vehicleVm.Owners = helper.GetAllOwners();
            vehicleVm.Makes = GetAllMakes();
            vehicleVm.Models = vehicleVm.make_id != 0 ? GetMakeModels(vehicleVm.make_id, vehicleVm.model_id) : new List<SelectListItem>();
            vehicleVm.Years = GetModelsYears();
            vehicleVm.BodyHulls = helper.GetGeneralCatalogValues(Enums.GeneralCatalog.VehicleBodyHull);
            vehicleVm.Colors = helper.GetGeneralCatalogValues(Enums.GeneralCatalog.Colors);
            vehicleVm.Fuels = helper.GetGeneralCatalogValues(Enums.GeneralCatalog.Fuels);
            vehicleVm.Types = helper.GetGeneralCatalogValues(Enums.GeneralCatalog.Types, false);
        }

        private IEnumerable<SelectListItem> GetAllMakes()
        {
            var selectList = new List<SelectListItem>();

            var vehMakesDataOp = new VehicleMakeData(_context);
            var makes = vehMakesDataOp.GetMakes();

            foreach (var make in makes)
            {
                selectList.Add(new SelectListItem
                {
                    Value = make.id.ToString(),
                    Text = make.name
                });
            }

            return selectList;
        }

        private IEnumerable<SelectListItem> GetMakeModels(short makeId, short modelId = 0)
        {
            var selectList = new List<SelectListItem>();

            var vehMakesDataOp = new VehicleMakeData(_context);
            var models = vehMakesDataOp.GetMakeModels(makeId);

            foreach (var model in models)
            {
                selectList.Add(new SelectListItem
                {
                    Value = model.id.ToString(),
                    Text = model.name,
                    Selected = model.id == modelId
                });
            }

            selectList.Insert(0, new SelectListItem { Value = "", Text = "- Please select a model -" });

            return selectList;
        }


        private IEnumerable<SelectListItem> GetModelsYears()
        {
            var selectList = new List<SelectListItem>();
            //selectList.Add( new SelectListItem{ Value = "", Text = "- Please select a year -" });

            var startYear = DateTime.Now.Year + 1;
            var endYear = DateTime.Now.Year - 15;

            for(var year = startYear; year >= endYear; year--)
            {
                selectList.Add(new SelectListItem
                {
                    Value = year.ToString(),
                    Text = year.ToString()
                });
            }

            return selectList;
        }
    }
}
