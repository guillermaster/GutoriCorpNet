using GutoriCorp.Data.Models;
using GutoriCorp.Models.BusinessViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GutoriCorpNet.Common.Enums;

namespace GutoriCorp.Data.Operations
{
    public class TicketData
    {
        private readonly ApplicationDbContext _context;

        public TicketData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(TicketViewModel ticketVm)
        {
            var ticket = GetEntity(ticketVm);
            ticket.paid = false;
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public TicketViewModel Get(long id)
        {
            var paymentsQry = QueryAllData();
            return paymentsQry.FirstOrDefault(p => p.id == id);
        }

        public TicketViewModel GetTicketFile(long id)
        {
            var ticket = _context.Ticket.Select(t => 
                        new TicketViewModel {
                            id = t.id,
                            ticket_num = t.ticket_num,
                            ticket_file = t.ticket_file,
                            ticket_file_type = t.ticket_file_type,
                            ticket_file_name = t.ticket_file_name,
                        }).FirstOrDefault(t => t.id == id);
            if (ticket == null) return null;

            return ticket;
        }

        public decimal GetPendingTicketsAmount(int vehicleId, DateTime duedate)
        {
            var pendingAmount = _context.Ticket.Where(t => t.ticket_date <= duedate && t.paid == false && t.vehicle_id == vehicleId).Sum(t => t.fine_amount);
            return pendingAmount;
        }

        public async Task SetTicketsPaid(int vehicleId, DateTime duedate, bool paid)
        {
            var tickets = _context.Ticket.Where(t => t.ticket_date <= duedate && t.paid != paid && t.vehicle_id == vehicleId).ToList();
            tickets.ForEach(t => t.paid = paid);
            await _context.SaveChangesAsync();
        }

        private IQueryable<TicketViewModel> QueryAllData()
        {
            var paymentsQry = from tic in _context.Ticket
                              join veh in _context.Vehicle on tic.vehicle_id equals veh.id
                              join mak in _context.VehicleMake on veh.make_id equals mak.id
                              join mod in _context.VehicleMakeModel on veh.model_id equals mod.id
                              join stat in _context.GeneralCatalogValues on tic.status_id equals stat.id
                              select new TicketViewModel
                              {
                                  id = tic.id,
                                  ticket_num = tic.ticket_num,                                  
                                  vehicle_id = tic.vehicle_id,
                                  status_id = tic.status_id,
                                  status = stat.title,
                                  ticket_date = tic.ticket_date,
                                  tlc_plate = tic.tlc_plate,
                                  vin_code = tic.vin_code,
                                  description = tic.description,
                                  occurrence_place = tic.occurrence_place,
                                  fine_amount = tic.fine_amount,
                                  created_on = tic.created_on,
                                  created_by = tic.created_by,
                                  modified_on = tic.modified_on,
                                  modified_by = tic.modified_by,
                                  vehicle = new VehicleViewModel { tlc_plate = veh.tlc_plate, year = veh.year, make = mak.name, model = mod.name}
                              };
            return paymentsQry;
        }

        public TicketViewModel InitTicketForVehicle(int vehicleId)
        {
            var vehOp = new VehicleData(_context);
            var vehicle = vehOp.Get(vehicleId);

            var ticket = new TicketViewModel
            {
                vehicle_id = vehicle.id,
                tlc_plate = vehicle.tlc_plate,
                vin_code = vehicle.vin_code,
                vehicle = vehicle
            };

            return ticket;
        }

        private static Ticket GetEntity(TicketViewModel ticketVm)
        {
            var ticket = new Ticket
            {
                vehicle_id = ticketVm.vehicle_id,
                tlc_plate = ticketVm.tlc_plate,
                vin_code = ticketVm.vin_code,
                ticket_num = ticketVm.ticket_num,
                description = ticketVm.description,
                occurrence_place = ticketVm.occurrence_place,
                ticket_date = ticketVm.ticket_date,
                fine_amount = ticketVm.fine_amount,
                ticket_file = ticketVm.ticket_file,
                ticket_file_type = ticketVm.ticket_file_type,
                ticket_file_name = ticketVm.ticket_file_name,
                created_on = DateTime.Now,
                created_by = ticketVm.created_by,
                modified_on = DateTime.Now,
                modified_by = ticketVm.modified_by,
                status_id = (short)GeneralStatus.Active
            };

            return ticket;
        }
    }
}
