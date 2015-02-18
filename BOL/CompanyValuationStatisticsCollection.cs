using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class CompanyValuationStatisticsCollection : BaseCollection<CompanyValuationStatistics>
    {
        public CompanyValuationStatistics Find(string symbol)
        {
            return (_innerList.Find(delegate(CompanyValuationStatistics e)
            { return (e.Symbol == symbol); }));
        }

    }
}
