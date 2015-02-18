using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.BOL
{
    public class UsStockCollection : BaseCollection<UsStock>
    {
        public UsStock Find(string symbol, int year)
        {
            return (_innerList.Find(delegate(UsStock e)
            { return (e.Symbol == symbol && e.Year == year); }));
        }

    }
}
