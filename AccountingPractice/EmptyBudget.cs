using System.Collections.Generic;

namespace AccountingPractice
{
    public class EmptyBudget : IBudgetRepo
    {
        public List<Budget> GetAll()
        {
            
            return new List<Budget>();
        }
    }
}