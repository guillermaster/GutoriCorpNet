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
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GutoriCorp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ticket.ToListAsync());
        }

        // GET: Tickets/Details/5
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketDataOp = new TicketData(_context);
            var ticket = ticketDataOp.Get(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create(int id)
        {
            var ticketDataOp = new TicketData(_context);
            var ticket = ticketDataOp.InitTicketForVehicle(id);
            ticket.refer_url = Request.Headers["Referer"].ToString();
            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticket, IFormFile ticket_file)
        {
            if (ModelState.IsValid)
            {
                var ticketDataOp = new TicketData(_context);
                ticket.ticket_file = ConvertToBytes(ticket_file);
                ticket.ticket_file_type = ticket_file.ContentType;
                ticket.ticket_file_name = ticket_file.FileName;

                await ticketDataOp.Add(ticket);

                if (string.IsNullOrWhiteSpace(ticket.refer_url))
                    return RedirectToAction("Index");
                else
                    return Redirect(ticket.refer_url);
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.SingleOrDefaultAsync(m => m.id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Ticket ticket)
        {
            if (id != ticket.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.SingleOrDefaultAsync(m => m.id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ticket = await _context.Ticket.SingleOrDefaultAsync(m => m.id == id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public FileResult DownloadTicketFile(long id)
        {
            var ticketDataOp = new TicketData(_context);
            var ticket = ticketDataOp.GetTicketFile(id);

            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            return File(ticket.ticket_file, ticket.ticket_file_type, ticket.ticket_file_name);
        }

        private bool TicketExists(long id)
        {
            return _context.Ticket.Any(e => e.id == id);
        }

        private byte[] ConvertToBytes(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
