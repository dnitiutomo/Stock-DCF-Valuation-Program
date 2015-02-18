using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface IIncomeStatementDA
    {
        IncomeStatement GetIncomeStatement(string tickerSymbol, int year);
        IncomeStatementCollection GetIncomeStatements(string tickerSymbol, int fromYear, int toYear);
        int CountIncomeStatement(string tickerSymbol, int year);
        int GetLastIncomeStatementYear(string tickerSymbol);
        void InsertIncomeStatement(IncomeStatement inc);
        void UpdateIncomeStatement(IncomeStatement inc);
    }
}
