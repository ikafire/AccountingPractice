using System;

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
            return 0.0d;
        }
    }
}
