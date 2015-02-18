using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface ICompanyDA
    {
        Company GetCompany(string tickerSymbol);
        CompanyCollection GetCompanies();
        int CountCompany(string tickersymbol);
        void InsertCompany(Company company);
        void UpdateCompany(Company company);
    }
}
