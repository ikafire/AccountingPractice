using System;
using System.Collections.Generic;
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
        public void MutilpleDaysInOneMonth()
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
        public void TwoMonths()
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

        [Fact]
        public void TwoMonthsWithOneEmptyMonth()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 31),
                new DateTime(2019, 2, 1),
                1);
        }

        [Fact]
        public void TwoMonthsWithOneEmptyMonthAndExtraData()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
                new Budget("201903", 31),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 31),
                new DateTime(2019, 2, 1),
                1);
        }

        [Fact]
        public void TwoMonthsWithFirstDay()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
                new Budget("201902", 28),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 1),
                new DateTime(2019, 2, 1),
                32);
        }

        [Fact]
        public void TwoMonthsWithLeapYear()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("202001", 31),
                new Budget("202002", 290),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2020, 1, 31),
                new DateTime(2020, 2, 1),
                11);
        }

        [Fact]
        public void ThreeMonths()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201901", 31),
                new Budget("201902", 280),
                new Budget("201903", 31000),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 31),
                new DateTime(2019, 3, 1),
                1281);
        }

        [Fact]
        public void CrossYear()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201812", 31),
                new Budget("201901", 310),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2018, 12, 31),
                new DateTime(2019, 1, 2),
                21);
        }

        [Fact]
        public void StartLargerThanEnd()
        {
            GivenDatabaseHasData(new List<Budget>
            {
                new Budget("201812", 31),
                new Budget("201901", 310),
            });

            Accounting = new Accounting(BudgetRepo);
            TotalAmountShouldBe(
                new DateTime(2019, 1, 2),
                new DateTime(2018, 12, 31),
                0);
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
