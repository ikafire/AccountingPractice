using System;
using System.Collections.Generic;
using Xunit;

namespace AccountingPractice
{
    public class AccountingTest
    {
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
}
