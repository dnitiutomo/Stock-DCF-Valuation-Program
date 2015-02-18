using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;
using System.Xml;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public class BalanceSheetBL
    {
        private IBalanceSheetDA _dao; //Data Access object for Balance Sheet

        #region CONSTRUCTORS

        private static BalanceSheetBL instance;

        public static BalanceSheetBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BalanceSheetBL();
                }
                return instance;
            }
        }

        private BalanceSheetBL(IBalanceSheetDA dao)
        { _dao = dao; }

        //Constructor injection - explicitly declares the dependencies of this class
        private BalanceSheetBL()
            : this(new BalanceSheetDA())
        { }

        #endregion

        #region PUBLIC METHODS

        public BalanceSheet GetBalanceSheet(string tickerSymbol, int year)
        { return _dao.GetBalanceSheet(tickerSymbol, year); }

        public BalanceSheetCollection GetBalanceSheets(string tickerSymbol, int fromYear, int toYear)
        { return _dao.GetBalanceSheets(tickerSymbol, fromYear, toYear); }

        #endregion

        #region UPDATE METHODS

        public void UpdateBalanceSheet(BalanceSheet bs)
        {
            if (BalanceSheetExists(bs.Symbol, bs.Year))
            {
                _dao.UpdateBalanceSheet(bs);
            }
            else
            {
                _dao.InsertBalanceSheet(bs);
            }
        }

        #endregion

        #region HELPER METHODS

        internal bool BalanceSheetExists(string Symbol, int year)
        {
            return _dao.CountBalanceSheet(Symbol, year) > 0;
        }

        #endregion
    }
}
