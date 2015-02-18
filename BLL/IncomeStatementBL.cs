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
    public class IncomeStatementBL
    {
        private IIncomeStatementDA _dao; //Data Access object for Income Statement

        #region CONSTRUCTORS

        private static IncomeStatementBL instance;

        public static IncomeStatementBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IncomeStatementBL();
                }
                return instance;
            }
        }

        private IncomeStatementBL(IIncomeStatementDA dao)
        { _dao = dao; }

        //Constructor injection - explicitly declares the dependencies of this class
        private IncomeStatementBL()
            : this(new IncomeStatementDA())
        { }

        #endregion

        #region PUBLIC METHODS

        public IncomeStatement GetIncomeStatement(string tickerSymbol, int year)
        { return _dao.GetIncomeStatement(tickerSymbol, year); }

        public IncomeStatementCollection GetIncomeStatements(string tickerSymbol, int fromYear, int toYear)
        { return _dao.GetIncomeStatements(tickerSymbol, fromYear, toYear); }

        #endregion

        #region UPDATE METHODS

        public void UpdateIncomeStatement(IncomeStatement inc)
        {
            if (IncomeStatementExists(inc.Symbol, inc.Year))
            {
                _dao.UpdateIncomeStatement(inc);
            }
            else
            {
                _dao.InsertIncomeStatement(inc);
            }
        }

        #endregion

        #region HELPER METHODS

        public bool IncomeStatementExists(string tickerSymbol, int year)
        {
            return _dao.CountIncomeStatement(tickerSymbol, year) > 0;
        }

        public int GetLastYear(string tickerSymbol)
        {
            return _dao.GetLastIncomeStatementYear(tickerSymbol);
        }

        #endregion
    }
}
