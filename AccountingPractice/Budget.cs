namespace AccountingPractice
{
    public class Budget
    {
        public Budget(string yearMonth, int amount)
        {
            YearMonth = yearMonth;
            Amount = amount;
        }

        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
}
