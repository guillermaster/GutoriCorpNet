using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GutoriCorp.Models.BusinessViewModels;
using GutoriCorp.Data.Models;
using static GutoriCorpNet.Common.Enums;

namespace GutoriCorp.Data.Operations
{
    public class PaymentData
    {
        private readonly ApplicationDbContext _context;

        public PaymentData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(PaymentViewModel payment)
        {
            _context.Add(GetEntity(payment));
            await _context.SaveChangesAsync();
        }

        public PaymentViewModel Get(long id, long contractId)
        {
            var paymentsQry = QueryAllData();
            return paymentsQry.FirstOrDefault(p => p.contract_id == contractId && p.id == id);
        }

        public async Task Update(long id, long contractId, short userId, decimal ticketsFee,  
            decimal totalPaid, decimal prevBalance, decimal prevCredit)
        {
            var payment = _context.Payment.FirstOrDefault(p => p.contract_id == contractId && p.id == id);
            if (payment == null)
            {
                throw new KeyNotFoundException();
            }

            payment.tickets = ticketsFee > 0;
            payment.tickets_fee = ticketsFee;
            payment.previous_balance = prevBalance;
            payment.previous_credit = prevCredit;
            payment.total_due_amount = GetTotalDueAmount(GetViewModel(payment));
            payment.total_paid_amount = totalPaid;
            payment.balance = GetBalance(payment.total_due_amount, payment.total_paid_amount);
            payment.credit = GetCredit(payment.total_due_amount, payment.total_paid_amount);
            payment.modified_by = userId;
            payment.modified_on = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public List<PaymentViewModel> GetAll(long contractId)
        {
            var paymentsQry = QueryAllData();
            return paymentsQry.Where(p => p.contract_id == contractId).ToList();
        }

        private IQueryable<PaymentViewModel> QueryAllData()
        {
            var paymentsQry = from pay in _context.Payment
                               join con in _context.Contract on pay.contract_id equals con.id
                               join own in _context.Owner on con.lessor_id equals own.id
                               join driv in _context.Driver on con.lessee_id equals driv.id
                               join frec in _context.GeneralCatalogValues on con.frequency_id equals frec.id
                               join stat in _context.GeneralCatalogValues on pay.status_id equals stat.id
                               select new PaymentViewModel
                               {
                                   id = pay.id,
                                   contract_id = con.id,
                                   lessor = own.ToString(),
                                   lessee = driv.ToString(),
                                   late = pay.late,
                                   tickets = pay.tickets,
                                   //frequency_id = con.frequency_id ?? 0,
                                   frequency = frec.title,
                                   due_date = pay.due_date,
                                   //due_day = con.due_day ?? 1,
                                   rental_fee = pay.rental_fee,
                                   late_fee = pay.late_fee,
                                   tickets_fee = pay.tickets_fee,
                                   total_due_amount = pay.total_due_amount,
                                   total_paid_amount = pay.total_paid_amount,
                                   balance = pay.balance,
                                   credit = pay.credit,
                                   status_id = pay.status_id,
                                   status = stat.title,
                                   payment_date = pay.payment_date,
                                   period = pay.period,
                                   created_on = pay.created_on,
                                   created_by = pay.created_by,
                                   modified_on = pay.modified_on,
                                   modified_by = pay.modified_by
                               };
            return paymentsQry;
        }

        public PaymentViewModel GetNextPaymentPeriodInit(long contractId)
        {
            var contractDataOp = new ContractData(_context);
            var contract = contractDataOp.Get(contractId);
            
            DateTime nextDueDate;
                        
            var latestPayment = _context.Payment.Where(p => p.contract_id == contractId).OrderByDescending(p => p.id)
                            .Select(p => 
                                new Payment {
                                    id = p.id,
                                    period = p.period,
                                    payment_date = p.payment_date,
                                    due_date = p.due_date
                                })
                            .FirstOrDefault();

            if(latestPayment == null)
            {
                nextDueDate = contract.contract_date;

                switch (contract.frequency_id)
                {
                    case (short)PaymentFrequency.Weekly:
                        var weekDayNum = (int)nextDueDate.DayOfWeek;
                        var daysToAdd = ((contract.due_day ?? 1) - weekDayNum) + 7;
                        nextDueDate = nextDueDate.AddDays(daysToAdd);
                        break;
                    case (short)PaymentFrequency.Monthly:
                        nextDueDate = nextDueDate.AddMonths(1);
                        nextDueDate = new DateTime(nextDueDate.Year, nextDueDate.Month, contract.due_day ?? 1);
                        break;
                    default:
                        throw new System.IO.InvalidDataException("Invalid contract payment frequency");
                }
            }
            else
            {
                switch (contract.frequency_id)
                {
                    case (short)PaymentFrequency.Weekly:
                        nextDueDate = latestPayment.due_date.AddDays(7);
                        break;
                    case (short)PaymentFrequency.Monthly:
                        nextDueDate = latestPayment.due_date.AddMonths(1);
                        break;
                    default:
                        throw new System.IO.InvalidDataException("Invalid contract payment frequency");
                }
            }

            short nextPeriod = 1;
            nextPeriod += (latestPayment != null ? latestPayment.period : (short)0);

            var nextPayment = GetViewModel(contract);
            nextPayment.period = nextPeriod;
            nextPayment.due_date = nextDueDate;

            if(latestPayment != null)
            {
                nextPayment.previous_credit = latestPayment.credit;
                nextPayment.previous_balance = latestPayment.balance;
                nextPayment.tickets_fee = GetTicketsAmount(nextPayment.vehicle_id, nextPayment.due_date);
                nextPayment.tickets = nextPayment.tickets_fee > 0;
            }

            nextPayment.late = DateTime.Now > nextPayment.due_date;

            if (!nextPayment.late)
            {
                nextPayment.late_fee = 0;
            }

            nextPayment.total_due_amount = GetTotalDueAmount(nextPayment);

            return nextPayment;
        }

        private decimal GetTicketsAmount(int vehicleId, DateTime paymentPeriod)
        {
            var ticketsDataOp = new TicketData(_context);
            var ticketsAmount = ticketsDataOp.GetPendingTicketsAmount(vehicleId, paymentPeriod);
            return ticketsAmount;
        }

        private static decimal GetTotalDueAmount(PaymentViewModel paymentVm)
        {
            var total_due_amount = paymentVm.rental_fee + paymentVm.late_fee + 
                paymentVm.tickets_fee + paymentVm.previous_balance - paymentVm.previous_credit;
            return total_due_amount;
        }

        private static decimal GetBalance(decimal totDueAmount, decimal totPaidAmount)
        {
            return totDueAmount > totPaidAmount ?
                            totDueAmount - totPaidAmount : 0;
        }

        private static decimal GetCredit(decimal totDueAmount, decimal totPaidAmount)
        {
            return totPaidAmount > totDueAmount ?
                            totPaidAmount - totDueAmount : 0;
        }

        public static PaymentViewModel GetViewModel(ContractViewModel contract)
        {
            var paymentVm = new PaymentViewModel
            {
                contract_id = contract.id,
                lessor = contract.lessor,
                lessee = contract.lessee,
                due_day = contract.due_day ?? 0,
                frequency_id = contract.frequency_id ?? 0,
                rental_fee = contract.rental_fee ?? 0,
                late_fee = contract.late_fee ?? 0,
                vehicle_id = contract.vehicle_id ?? 0
            };

            return paymentVm;
        }

        private static Payment GetEntity(PaymentViewModel paymentVm)
        {
            var payment = new Payment
            {
                contract_id = paymentVm.contract_id,
                period = paymentVm.period,
                due_date = paymentVm.due_date,
                payment_date = DateTime.Now,
                late = paymentVm.late,
                tickets = paymentVm.tickets,
                rental_fee = paymentVm.rental_fee,
                late_fee = paymentVm.late_fee,
                tickets_fee = paymentVm.tickets_fee,
                total_due_amount = GetTotalDueAmount(paymentVm),
                total_paid_amount = paymentVm.total_paid_amount,
                status_id = (short)GeneralStatus.Active,
                created_on = DateTime.Now,
                created_by = paymentVm.created_by,
                modified_on = DateTime.Now,
                modified_by = paymentVm.modified_by
            };

            paymentVm.balance = GetBalance(payment.total_due_amount, payment.total_paid_amount);
            paymentVm.credit = GetCredit(payment.total_due_amount, payment.total_paid_amount);

            return payment;
        }

        private static PaymentViewModel GetViewModel(Payment payment)
        {
            var paymentVm = new PaymentViewModel
            {
                rental_fee = payment.rental_fee,
                late_fee = payment.late_fee,
                tickets_fee = payment.tickets_fee,
                previous_balance = payment.previous_balance,
                credit = payment.previous_credit
            };
            return paymentVm;
        }
    }
}
