using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class IncomeStatementCollection : BaseCollection<IncomeStatement>
    {
        public IncomeStatement Find(int year)
        {
            return (_innerList.Find(delegate(IncomeStatement e)
            { return (e.Year == year); }));
        }

        public IncomeStatementCollection FindRange(int fromYear, int toYear)
        {
            List<IncomeStatement> lists =
                _innerList.FindAll(delegate(IncomeStatement p)
                { return (p.Year >= fromYear && p.Year <= toYear); });

            if (lists == null) return null;

            IncomeStatementCollection incs = new IncomeStatementCollection();
            incs.AddRange(lists);
            return incs;
        }
    }
}
