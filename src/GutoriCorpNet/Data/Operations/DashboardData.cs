using GutoriCorpNet.Common;
using GutoriCorp.Models.BusinessViewModels;
using GutoriCorp.Models.GeneralViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Operations
{
    public class DashboardData
    {
        private readonly ApplicationDbContext _context;

        public DashboardData(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DashboardSerieViewModel> GetPaymentsSummaryPerWeek()
        {
            var series = new List<DashboardSerieViewModel>();
            var weekStartEnd = Dates.GetWeekBeginEndDates(DateTime.Now);

            // query paid amount
            var payments = (from pay in _context.Payment
                            join con in _context.Contract on pay.contract_id equals con.id
                            where
                            con.frequency_id == (short)Enums.PaymentFrequency.Weekly &&
                            pay.status_id == (short)Enums.GeneralStatus.Active &&
                            pay.payment_date >= weekStartEnd.Item1 &&
                            pay.payment_date < weekStartEnd.Item2
                            select new PaymentViewModel
                            {
                                id = pay.id,
                                contract_id = con.id,
                                total_paid_amount = pay.total_paid_amount
                            }).ToList();
            //var payments = _context.Payment.Where(p =>
            //                                p.status_id == (short)Enums.GeneralStatus.Active &&
            //                                p.payment_date >= weekStartEnd.Item1 &&
            //                                p.payment_date <= weekStartEnd.Item2).ToList();

            var paymentsTotAmount = payments.Sum(p => p.total_paid_amount);
            series.Add(
                new DashboardSerieViewModel {
                    Title = "Collected payments",
                    Value = paymentsTotAmount.ToString(),
                    Color = "#FFC107"
            });

            // query pending amount
            var contracts = _context.Contract.
                                Where(c => c.frequency_id == (short)Enums.PaymentFrequency.Weekly && !payments.Any(p => p.contract_id == c.id)).
                                Select(c => new ContractViewModel { id = c.id, rental_fee = c.rental_fee }).ToList();
            var pendingTotAmount = contracts.Sum(c => c.rental_fee);

            var ticketsInPendingCont = (from tic in _context.Ticket
                                        join con in _context.Contract on tic.vehicle_id equals con.vehicle_id
                                        where contracts.Any(c => c.id == con.id) && !payments.Any(p => p.contract_id == con.id)
                                        select tic.fine_amount).Sum();

            pendingTotAmount += ticketsInPendingCont;
            series.Add(
                new DashboardSerieViewModel
                {
                    Title = "Pending",
                    Value = pendingTotAmount.ToString(),
                    Color = "#009688"
                });

            return series;
        }

        public List<DashboardSerieViewModel> GetPaymentsSummaryPerMonth()
        {
            var series = new List<DashboardSerieViewModel>();
            var periodStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var periodEnd = DateTime.Now.Month == 12 ?
                                        new DateTime(DateTime.Now.Year + 1, 1, 1) :
                                        new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);

            // query paid amount
            var payments = (from pay in _context.Payment
                            join con in _context.Contract on pay.contract_id equals con.id
                            where
                            con.frequency_id == (short)Enums.PaymentFrequency.Monthly &&
                            pay.status_id == (short)Enums.GeneralStatus.Active &&
                            pay.payment_date >= periodStart &&
                            pay.payment_date < periodEnd
                            select new PaymentViewModel
                            {
                                id = pay.id,
                                contract_id = con.id,
                                total_paid_amount = pay.total_paid_amount
                            }).ToList();

            //var payments = _context.Payment.Where(p =>
            //                                p.status_id == (short)Enums.GeneralStatus.Active &&
            //                                p.payment_date >= periodStart &&
            //                                p.payment_date < periodEnd).ToList();

            var paymentsTotAmount = payments.Sum(p => p.total_paid_amount);
            series.Add(
                new DashboardSerieViewModel
                {
                    Title = "Collected payments",
                    Value = paymentsTotAmount.ToString(),
                    Color = "#FFC107"
                });

            // query pending amount
            var contracts = _context.Contract.
                               Where(c => c.frequency_id == (short)Enums.PaymentFrequency.Monthly && !payments.Any(p => p.contract_id == c.id)).
                               Select(c => new ContractViewModel { id = c.id, rental_fee = c.rental_fee }).ToList();
            var pendingTotAmount = contracts.Sum(c => c.rental_fee);

            var ticketsInPendingCont = (from tic in _context.Ticket
                                        join con in _context.Contract on tic.vehicle_id equals con.vehicle_id
                                        where contracts.Any(c => c.id == con.id) && !payments.Any(p => p.contract_id == con.id)
                                        select tic.fine_amount).Sum();

            pendingTotAmount += ticketsInPendingCont;
            series.Add(
                new DashboardSerieViewModel
                {
                    Title = "Pending",
                    Value = pendingTotAmount.ToString(),
                    Color = "#009688"
                });

            return series;
        }

        public int GetTotalActiveVehicles()
        {
            var totVeh = _context.Vehicle.Count(v => v.status_id == (short)Enums.GeneralStatus.Active);
            return totVeh;
        }

        public int GetTotalFreeVehicles()
        {
            var totFreeVeh = _context.Vehicle.Count(v => v.status_id == (short)Enums.GeneralStatus.Active && v.driver_id == null);
            return totFreeVeh;
        }

        public int GetTotalActiveContracts()
        {
            var totActCont = _context.Contract.Count(c => c.status_id == (short)Enums.GeneralStatus.Active);
            return totActCont;
        }

        public decimal GetTotalIncomeLastMonth()
        {
            var currMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var lastMonthFirstDay = DateTime.Now.Month == 1 ? 
                                        new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1) :
                                        new DateTime(DateTime.Now.Year-1, 12, 1);

            var totPayments = _context.Payment.Where(p => p.payment_date < currMonthFirstDay && p.payment_date >= lastMonthFirstDay).Sum(p => p.total_paid_amount);
            return totPayments;
        }
    }
}
