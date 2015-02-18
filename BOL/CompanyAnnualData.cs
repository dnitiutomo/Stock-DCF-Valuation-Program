using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class CompanyAnnualData : FinancialStatement
    {
        #region PRIVATE MEMBERS

        private decimal _LEVERED_BETA;
        private decimal _COST_OF_DEBT;
        private decimal _SHARES_OUTSTANDING;
        private decimal _DIVIDEND_YIELD;

        #endregion

        #region PUBLIC PROPERTIES

        public decimal LeveredBeta
        { get { return _LEVERED_BETA; } set { _LEVERED_BETA = value; } }

        public decimal CostOfDebt
        { get { return _COST_OF_DEBT; } set { _COST_OF_DEBT = value; } }

        public decimal SharesOutstanding
        { get { return _SHARES_OUTSTANDING; } set { _SHARES_OUTSTANDING = value; } }

        public decimal DividendYield
        { get { return _DIVIDEND_YIELD; } set { _DIVIDEND_YIELD = value; } }


        #endregion

        #region CONSTRUCTORS

        public CompanyAnnualData()
            : base()
        {
            Initialize();
        }

        public CompanyAnnualData(string tickerSymbol, int year, decimal beta, decimal costOfDebt, decimal shares, decimal yield)
            : base()
        {
            this._SYMBOL = tickerSymbol;
            this._YEAR = year;
            this._LEVERED_BETA = beta;
            this._COST_OF_DEBT = costOfDebt;
            this._SHARES_OUTSTANDING = shares;
            this._DIVIDEND_YIELD = yield;
        }

        #endregion

        #region HELPER METHODS

        private void Initialize()
        {
            this._SYMBOL = string.Empty;
            this._YEAR = 0;
            this._LEVERED_BETA = 0;
            this._COST_OF_DEBT = 0;
            this._SHARES_OUTSTANDING = 0;
            this._DIVIDEND_YIELD = 0;
        }

        #endregion
    }
}

