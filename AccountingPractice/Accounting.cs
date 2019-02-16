using System;
using System.Collections.Generic;
using System.Data;
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

            var tempStart = start;
            while (tempStart <= end)
            {
                if (tempStart.ToString("yyyyMM") != end.ToString("yyyyMM"))
                {
                    var tempEnd = new DateTime(tempStart.Year, tempStart.Month,
                        DateTime.DaysInMonth(tempStart.Year, tempStart.Month));
                  list.Add(new MonthlyRate(){YearMonth = tempStart.ToString("yyyyMM"), Rate = GetRate(tempStart, tempEnd) });
                  tempStart = tempEnd.AddDays(1);
                }
                else
                {

                    list.Add(new MonthlyRate() { YearMonth = tempStart.ToString("yyyyMM"), Rate = GetRate(tempStart, end) });
                    break;
                }
            }

            return list;

            //if (start.Year == end.Year && start.Month == end.Month)
            //{
            //    var yearMonth = ToYearMonth(start);
            //    var rate = GetRate(start, end);
            //    return  new List<MonthlyRate>(){new MonthlyRate(){Rate = rate, YearMonth = yearMonth}};
            //}
            //else
            //{
            //    var startYearMonth = ToYearMonth(start);
            //    var startRate = GetRate(start,
            //        new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month)));

            //    var endYearMonth = ToYearMonth(end);
            //    var endRate = GetRate(new DateTime(end.Year, end.Month, 1), end);

            //    return new List<MonthlyRate>()
            //    {
            //        new MonthlyRate() { Rate = startRate, YearMonth = startYearMonth },
            //        new MonthlyRate()
            //        {
            //            Rate = endRate,
            //            YearMonth = endYearMonth,
            //        }
            //    };
            //}
        }

        private double CalculateBudget(MonthlyRate monthlyRate, List<Budget> budgets)
        {
            return budgets.FirstOrDefault(x => x.YearMonth == monthlyRate.YearMonth)?.Amount* monthlyRate.Rate ?? 0;
        }

        private static double GetRate(DateTime start, DateTime end)
        {
            int monthdays = DateTime.DaysInMonth(start.Year, start.Month);
            int totalDays = (int) end.Subtract(start).TotalDays + 1;
            double rate = (double) totalDays / monthdays;
            return rate;
        }

        private static string ToYearMonth(DateTime start)
        {
            var yearMonth = start.ToString("yyyyMM");
            return yearMonth;
        }
    }

    public class MonthlyRate
    {
        public string YearMonth { get; set; }
        public double Rate { get; set; }


    }
}
