using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;

namespace StockValuationLibrary._2.BLL
{
    public class UsStockBL
    {
        private IUsStocksDA _dao;
        private static UsStockBL instance;

        public static UsStockBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsStockBL();
                }
                return instance;
            }
        }

        private UsStockBL(IUsStocksDA dao)
        { _dao = dao; }

        private UsStockBL()
            : this(new UsStockDA())
        { }

        #region PUBLIC METHODS

        public UsStock GetUsStock(string tickerSymbol, int year)
        { return _dao.GetUsStock(tickerSymbol, year); }

        public UsStockCollection GetUsStocks(int year)
        { return _dao.GetUsStocks(year); }

        public UsStockCollection GetUsStocks(string tickerSymbol, int fromYear, int toYear)
        { return _dao.GetUsStocks(tickerSymbol, fromYear, toYear); }

        #endregion

        #region UPDATE METHODS

        public void UpdateUsStock(UsStock stock)
        {
            if (UsStockExists(stock.Symbol, stock.Year))
            {
                //_dao.UpdateUsStock(stock);
            }
            else
            {
                _dao.InsertUsStock(stock);
            }
            
        }

        public bool UsStockExists(string symbol,int year)
        {
            if(_dao.CountUsStock(symbol, year) > 0)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
