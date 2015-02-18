using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class BalanceSheetCollection : BaseCollection<BalanceSheet>
    {
        public BalanceSheet Find(int year)
        {
            return (_innerList.Find(delegate(BalanceSheet e)
            { return (e.Year == year); }));
        }

    }
}
