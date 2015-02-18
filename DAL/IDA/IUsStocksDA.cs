using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface IUsStocksDA
    {
        UsStock GetUsStock(string tickerSymbol, int year);
        UsStockCollection GetUsStocks(string tickerSymbol, int fromYear, int toYear);
        int CountUsStock(string tickerSymbol, int year);
        void InsertUsStock(UsStock bs);
        void UpdateUsStock(UsStock bs);
        UsStockCollection GetUsStocks(int year);
        List<string> GetCompanyList();
        bool IsRevenuePopulated(string symbol);
    }
}
