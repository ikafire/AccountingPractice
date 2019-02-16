using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace AccountingPractice
{
    public class AccountingTest
    {
        private IBudgetRepo BudgetRepo { get; set; }

        private Accounting Accounting { get; set; }

        [Fact]
        public void NoData()
        {
            GivenDatabaseHasData(new List<Budget>());
            Accounting = new Accounting(BudgetRepo);

            TotalAmountShouldBe(
                new DateTime(2019, 1, 1),
                new DateTime(2019, 1, 31),
                0);
        }

        [Fact]
        public void NoDataInMonth()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201902", 1),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 1),
                new DateTime(2019, 1, 31),
                0);
        }

        [Fact]
        public void WholeMonth()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 1),
                new DateTime(2019, 1, 31),
                31);
        }

        [Fact]
        public void MutilDaysInOneMonth()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 1),
                new DateTime(2019, 1, 10),
                10);
        }

        [Fact]
        public void MutilDaysInTwoMonth()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
                new Budget("201902",280)
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 31),
                new DateTime(2019, 2, 1),
                11);
        }

        [Fact]
        public void OneDay()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 1),
                new DateTime(2019, 1, 1),
                1);
        }

        private void TotalAmountShouldBe(DateTime start, DateTime end, double expected)
        {
            Accounting.TotalAmount(start, end).Should().Be(expected);
        }

        private void GivenDatabaseHasData(List<Budget> data)
        {
            var budgetRepoMock = new Mock<IBudgetRepo>();
            budgetRepoMock.Setup(x => x.GetAll()).Returns(data);
            BudgetRepo = budgetRepoMock.Object;
        }
    }
}