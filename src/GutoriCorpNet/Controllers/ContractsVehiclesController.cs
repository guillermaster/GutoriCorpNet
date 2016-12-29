using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GutoriCorp.Models.BusinessViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using GutoriCorp.Data.Operations;
using GutoriCorp.Data;
using System.Data.Entity.Infrastructure;

namespace GutoriCorp.Controllers
{
    public class ContractsVehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractsVehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContractsVehicles
        public ActionResult Index()
        {
            return View();
        }

        // GET: ContractsVehicles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        

        // GET: ContractsVehicles/Edit/5
        public ActionResult Edit(long id)
        {
            var model = GetViewModel(id);
            SetVehiclesOptions(model);
            return View(model);
        }

        // POST: ContractsVehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, ContractVehicleViewModel model)
        {
            if (id != model.contract_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // set vehicle in contract
                    var contractDataOp = new ContractData(_context);
                    contractDataOp.SetVehicle(id, model.vehicle_id, 1);

                    // set driver in vehicle
                    var vehicleDataOp = new VehicleData(_context);
                    vehicleDataOp.SetDriver(model.vehicle_id, model.lessee_id, 1);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!VehicleExists(vehicle.id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                        throw;
                    //}
                }
                return RedirectToAction("Index", "Contracts");
            }
            return View(model);
        }

        // GET: ContractsVehicles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContractsVehicles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private ContractVehicleViewModel GetViewModel(long contractId)
        {
            var contractDataOp = new ContractData(_context);
            var contract = contractDataOp.Get(contractId);
            var model = new ContractVehicleViewModel
            {
                contract_id = contract.id,
                lessee_id = contract.lessee_id,
                LesseeName = contract.lessee,
                LessorName = contract.lessor,
                vehicle_id = contract.vehicle_id ?? 0
            };

            return model;
        }

        private void SetVehiclesOptions(ContractVehicleViewModel model)
        {
            // set the list of vehicle options
            var vehicleDataOp = new VehicleData(_context);
            var vehicles = vehicleDataOp.GetAllFree(true);
            
            var vehiclesOptions = new List<SelectListItem>();

            if (vehicles.Count > 0)
            {
                vehiclesOptions.Add(new SelectListItem
                {
                    Value = string.Empty,
                    Text = " - Select -"
                });
            }
            else
            {
                if (model.vehicle_id > 0)
                {
                    var assignedVehicle = vehicleDataOp.Get(model.vehicle_id);
                    vehiclesOptions.Add(new SelectListItem
                    {
                        Value = assignedVehicle.id.ToString(),
                        Text = assignedVehicle.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    vehiclesOptions.Add(new SelectListItem
                    {
                        Value = string.Empty,
                        Text = " - No available vehicles -"
                    });
                }
            }

            // add elements to the options list
            foreach (var vehicle in vehicles)
            {
                vehiclesOptions.Add(new SelectListItem
                {
                    Value = vehicle.id.ToString(),
                    Text = vehicle.ToString(),
                    Selected = vehicle.id == model.vehicle_id
                });
            }
            
            model.AvailableVehicles = vehiclesOptions;
        }
    }
}