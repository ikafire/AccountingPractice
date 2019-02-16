using System.Collections.Generic;

namespace AccountingPractice
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}
