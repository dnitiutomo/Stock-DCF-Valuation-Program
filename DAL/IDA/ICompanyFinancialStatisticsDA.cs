using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface ICompanyFinancialStatisticsDA
    {
        CompanyFinancialStatistics GetCompanyFinancialStatistics(string tickerSymbol, DateTime date);
        void InsertCompanyFinancialStatistics(CompanyFinancialStatistics comp);
        void UpdateCompanyFinancialStatistics(CompanyFinancialStatistics comp);
        int CountCompanyFinancialStatistics(string tickerSymbol, DateTime date);
    }
}
