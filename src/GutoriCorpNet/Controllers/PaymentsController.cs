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
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace GutoriCorp.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Payments
        public IActionResult Index(long id)
        {
            var paymentData = new PaymentData(_context);
            var payments = paymentData.GetAll(id);
            return View(payments);
        }

        // GET: Payments/Details/5
        public IActionResult Details(long? id, long? cid)
        {
            if (id == null || cid == null)
            {
                return NotFound();
            }

            var payment = GetPayment(id.Value, cid.Value);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(long id)
        {
            try
            {
                var paymentVm = InitPaymentForContract(id);
                return View(paymentVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel payment)
        {
            if (ModelState.IsValid)
            {
                var paymentData = new PaymentData(_context);
                payment.id = 0;
                payment.created_by = 1;
                payment.modified_by = 1;
                await paymentData.Add(payment);
                return RedirectToAction("Index", new { id = payment.contract_id });
            }

            // set tickets as paid
            if (payment.tickets)
            {
                var ticketsDataOp = new TicketData(_context);
                await ticketsDataOp.SetTicketsPaid(payment.vehicle_id, payment.due_date, true);
            }

            return View(payment);
        }

        // GET: Payments/Edit/5
        public IActionResult Edit(long? id, long? cid)
        {
            if (id == null || cid == null)
            {
                return NotFound();
            }

            var payment = GetPayment(id.Value, cid.Value);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, PaymentViewModel payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var paymentData = new PaymentData(_context);

                    await paymentData.Update(id, payment.contract_id, 1,
                        payment.tickets_fee, payment.total_paid_amount,
                        payment.previous_balance, payment.previous_credit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = payment.contract_id });
            }
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.id == id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PaymentExists(long id)
        {
            return _context.Payment.Any(e => e.id == id);
        }

        private PaymentViewModel InitPaymentForContract(long contractId)
        {            
            var paymentDataOp = new PaymentData(_context);
            var paymentVm = paymentDataOp.GetNextPaymentPeriodInit(contractId);

            return paymentVm;
        }

        private PaymentViewModel GetPayment(long id, long contractId)
        {
            var paymentDataOp = new PaymentData(_context);
            var paymentVm = paymentDataOp.Get(id, contractId);

            return paymentVm;
        }
    }
}
