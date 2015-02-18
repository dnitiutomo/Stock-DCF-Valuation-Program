using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class IncomeStatement : FinancialStatement
    {
        #region PRIVATE MEMBERS

        private decimal _REVENUE;
        private decimal _COGS;
        private decimal _OPERATING_EXPENSES;
        private decimal _DEPRECIATION;
        private decimal _NET_INCOME;
        private decimal _CAPITAL_EXPENDITURES;

        #endregion

        #region PUBLIC PROPERTIES

        public decimal Revenue
        { get { return _REVENUE; } set { _REVENUE = value; } }

        public decimal Cogs
        { get { return _COGS; } set { _COGS = value; } }

        public decimal OperatingExpenses
        { get { return _OPERATING_EXPENSES; } set { _OPERATING_EXPENSES = value; } }

        public decimal Depreciation
        { get { return _DEPRECIATION; } set { _DEPRECIATION = value; } }

        public decimal NetIncome
        { get { return _NET_INCOME; } set { _NET_INCOME = value; } }

        public decimal CapitalExpenditures
        { get { return _CAPITAL_EXPENDITURES; } set { _CAPITAL_EXPENDITURES = value; } }

        public decimal Ebit
        {
            get { return (_REVENUE - _COGS - _OPERATING_EXPENSES); }
        }

        #endregion

        #region CONSTRUCTORS

        public IncomeStatement()
            : base()
        {
            Initialize();
        }

        public IncomeStatement(string tickerSymbol, int year, decimal revenue, decimal cogs, decimal opExp,
    decimal depreciation, decimal ni, decimal capex)
            : base()
        {
            this._SYMBOL = tickerSymbol;
            this._YEAR = year;
            this._REVENUE = revenue;
            this._COGS = cogs;
            this._OPERATING_EXPENSES = opExp;
            this._DEPRECIATION = depreciation;
            this._NET_INCOME = ni;
            this._CAPITAL_EXPENDITURES = capex;
        }

        #endregion

        #region HELPER METHODS

        private void Initialize()
        {
            this._REVENUE = 0;
            this._COGS = 0;
            this._OPERATING_EXPENSES = 0;
            this._DEPRECIATION = 0;
            this._NET_INCOME = 0;
            this._CAPITAL_EXPENDITURES = 0;
        }

        public decimal NOP(decimal taxRate)
        {
            return Ebit * (1 - taxRate);
        }

        #endregion
    }
}
