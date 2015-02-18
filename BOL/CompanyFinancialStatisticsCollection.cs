using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.BOL
{
    public class CompanyFinancialStatisticsCollection : BaseCollection<CompanyFinancialStatistics>
    {
        public CompanyFinancialStatistics Find(string symbol)
        {
            return (_innerList.Find(delegate(CompanyFinancialStatistics e)
            { return (e.Symbol == symbol); }));
        }
    }
}
