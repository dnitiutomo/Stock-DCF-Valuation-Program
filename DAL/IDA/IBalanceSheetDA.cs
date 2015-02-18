using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface IBalanceSheetDA
    {
        BalanceSheet GetBalanceSheet(string tickerSymbol, int year);
        BalanceSheetCollection GetBalanceSheets(string tickerSymbol, int fromYear, int toYear);
        int CountBalanceSheet(string tickerSymbol, int year);
        void InsertBalanceSheet(BalanceSheet bs);
        void UpdateBalanceSheet(BalanceSheet bs);
    }
}
