using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class BalanceSheet : FinancialStatement
    {
        #region PRIVATE MEMBERS

        private decimal _CURRENT_ASSETS;
        private decimal _CURRENT_LIABILITIES;
        private decimal _DEBT;
        private decimal _SHAREHOLDER_EQUITY;
        private decimal _PPE;
        private decimal _CASH;
        private decimal _TOTAL_ASSETS;

        #endregion

        #region PUBLIC PROPERTIES

        public decimal CurrentAssets
        { get { return _CURRENT_ASSETS; } set { _CURRENT_ASSETS = value; } }

        public decimal CurrentLiabilities
        { get { return _CURRENT_LIABILITIES; } set { _CURRENT_LIABILITIES = value; } }

        public decimal Ppe
        { get { return _PPE; } set { _PPE = value; } }

        public decimal Cash
        { get { return _CASH; } set { _CASH = value; } }

        public decimal Debt
        { get { return _DEBT; } set { _DEBT = value; } }

        public decimal ShareholdersEquity
        { get { return _SHAREHOLDER_EQUITY; } set { _SHAREHOLDER_EQUITY = value; } }

        public decimal TotalAssets
        { get { return _TOTAL_ASSETS; } set { _TOTAL_ASSETS = value; } }

        public decimal TotalLiabilities
        {
            get { return _TOTAL_ASSETS - _SHAREHOLDER_EQUITY; }
        }

        public decimal WorkingCapital
        {
            get { return _CURRENT_ASSETS - _CURRENT_LIABILITIES; }
        }

        public decimal InvestedCapital
        {
            get { return _PPE + WorkingCapital; }
        }

        #endregion

        #region CONSTRUCTORS

        public BalanceSheet()
            : base()
        {
            Initialize();
        }

        public BalanceSheet(string tickerSymbol, int year, decimal currAssets, decimal currLiabilities,
            decimal debt, decimal ppe, decimal cash, decimal equity, decimal assets)
            : base()
        {
            this._SYMBOL = tickerSymbol;
            this._YEAR = year;
            this._CURRENT_ASSETS = currAssets;
            this._CURRENT_LIABILITIES = currLiabilities;
            this._DEBT = debt;
            this._PPE = ppe;
            this._CASH = cash;
            this._SHAREHOLDER_EQUITY = equity;
            this._TOTAL_ASSETS = assets;
        }

        #endregion

        #region HELPER METHODS

        private void Initialize()
        {
            this._CURRENT_ASSETS = 0;
            this._PPE = 0;
            this._CASH = 0;
            this._CURRENT_LIABILITIES = 0;
            this._DEBT = 0;
            this._SHAREHOLDER_EQUITY = 0;
            this._TOTAL_ASSETS = 0;
        }


        public decimal ExcessCash(decimal revenue)
        {
            decimal excessCash = _CASH - (revenue * (decimal)0.02);
            if (excessCash > 0)
            {
                return excessCash;
            }
            else
            {
                return 0;
            }
        }

        public decimal ROIC(decimal NOP)
        {
            return NOP / InvestedCapital;
        }
        #endregion
    }
}
