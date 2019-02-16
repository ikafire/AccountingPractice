using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingPractice
{
    public class Accounting
    {
        public IBudgetRepo BudgetRepo { get; }

        public Accounting(IBudgetRepo budgetRepo)
        {
            BudgetRepo = budgetRepo;
        }

        public double TotalAmount(DateTime start, DateTime end)
        {
            List<MonthlyRate> monthlyRates = GetMonthlyRates(start, end);
            var budgets = BudgetRepo.GetAll();
            return monthlyRates.Sum(x => CalculateBudget(x, budgets));
        }

        private List<MonthlyRate> GetMonthlyRates(DateTime start, DateTime end)
        {
            var list = new List<MonthlyRate>();
            while (start <= end)
            {
                var tempEnd = NotInSameMonth(start, end) ? LastDayOfMonth(start) : end;
                list.Add(new MonthlyRate() { YearMonth = start.ToString("yyyyMM"), Rate = GetRate(start, tempEnd) });
                start = tempEnd.AddDays(1);
            }

            return list;
        }

        private static DateTime LastDayOfMonth(DateTime start)
        {
            return new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
        }

        private static bool NotInSameMonth(DateTime start, DateTime end)
        {
            return start.ToString("yyyyMM") != end.ToString("yyyyMM");
        }

        private double CalculateBudget(MonthlyRate monthlyRate, List<Budget> budgets)
        {
            return budgets.FirstOrDefault(x => x.YearMonth == monthlyRate.YearMonth)?.Amount* monthlyRate.Rate ?? 0;
        }

        private static double GetRate(DateTime start, DateTime end)
        {
            int daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            int totalDays = (int) end.Subtract(start).TotalDays + 1;
            return (double) totalDays / daysInMonth;
        }

        private static string ToYearMonth(DateTime date)
        {
            return date.ToString("yyyyMM");
        }
    }

    public class MonthlyRate
    {
        public string YearMonth { get; set; }
        public double Rate { get; set; }
    }
}
