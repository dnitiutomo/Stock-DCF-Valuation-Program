using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface ICompanyAnnualDataDA
    {
        CompanyAnnualData GetCompanyAnnual(string tickerSymbol, int year);
        int CountCompanyAnnual(string tickerSymbol, int year);
        void InsertCompanyAnnual(CompanyAnnualData comp);
        void UpdateCompanyAnnual(CompanyAnnualData comp);
    }
}
