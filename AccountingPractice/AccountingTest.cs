using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AccountingPractice
{
    public class AccountingTest
    {
        [Fact]
        public void NoData()
        {
            var emptyBudget = new EmptyBudget();
            var accounting = new Accounting(emptyBudget);
            var actual = accounting.TotalAmount(new DateTime(2019, 1, 1), new DateTime(2019, 1, 31));
            actual.Should().Be(0);
        }
    }
}
