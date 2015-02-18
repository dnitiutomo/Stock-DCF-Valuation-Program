using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.BOL
{
    public class CompanyFinancialStatistics
    {
        private string _SYMBOL;
        private DateTime _DATE;
        private decimal _BOOK_VALUE_PER_SHARE;
        private decimal _STOCK_PRICE;
        private decimal _ROE;
        private decimal _ROA;
        private decimal _DEBT_TO_EQUITY;
        private decimal _YEAR_HIGH;
        private decimal _YEAR_LOW;
        private decimal _EPS;
        private decimal _REVENUE_PER_SHARE;
        private decimal _AVG_VOLUME;
        private decimal _MARKET_CAP;
        private decimal _TOTAL_DEBT;
        private decimal _SHORT_RATIO_CURRENT;
        private decimal _SHORT_RATIO_LAST_MONTH;
        private decimal _50_MA;
        private decimal _200_MA;
        private decimal _DIV_YIELD_PERCENT;
        private decimal _OPERATING_MARGIN;
        private decimal _PROFIT_MARGIN;
        private decimal _TRAILING_PE;
        private decimal _FORWARD_PE;
        private decimal _PEG_RATIO;
        private decimal _ENTERPRISE_VALUE;
        private decimal _PRICE_TO_SALES;
        private decimal _PRICE_TO_BOOK;
        private decimal _EV_REVENUE;
        private decimal _EV_EBITDA;
        private decimal _Q_REV_GROWTH;
        private decimal _EBITDA;
        private decimal _NI_COMMON_SHARES;
        private decimal _Q_EARNINGS_GROWTH;
        private decimal _TOTAL_CASH;
        private decimal _CURRENT_RATIO;
        private decimal _OP_CF;
        private decimal _LEVERED_CF;

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public DateTime Date
        { get { return _DATE; } set { _DATE = value; } }

        public decimal BookValuePerShare
        { get { return _BOOK_VALUE_PER_SHARE; } set { _BOOK_VALUE_PER_SHARE = value; } }

        public decimal StockPrice
        { get { return _STOCK_PRICE; } set { _STOCK_PRICE = value; } }

        public decimal ReturnOnEquity
        { get { return _ROE; } set { _ROE = value; } }

        public decimal ReturnOnAssets
        { get { return _ROA; } set { _ROA = value; } }

        public decimal DebtToEquity
        { get { return _DEBT_TO_EQUITY; } set { _DEBT_TO_EQUITY = value; } }

        public decimal Roic()
        {
            if (_ROE == 0)
            {
                return 0;
            }

            decimal deno = ((NetIncome() / _ROE) + _TOTAL_DEBT);
            if (deno != 0)
            {
                return ((NetIncome() - (_MARKET_CAP * _DIV_YIELD_PERCENT)) / ((NetIncome() / _ROE) + _TOTAL_DEBT));
            }
            else
            {
                return 0;
            }
        }

        public decimal NetIncome()
        {
            if (_EV_REVENUE != 0)
            {
                return ((_ENTERPRISE_VALUE / _EV_REVENUE) * _PROFIT_MARGIN);
            }
            else
            {
                return 0;
            }
        }

        public decimal YearHigh
        { get { return _YEAR_HIGH; } set { _YEAR_HIGH = value; } }

        public decimal YearLow
        { get { return _YEAR_LOW; } set { _YEAR_LOW = value; } }

        public decimal Eps
        { get { return _EPS; } set { _EPS = value; } }

        public decimal RevenuePerShare
        { get { return _REVENUE_PER_SHARE; } set { _REVENUE_PER_SHARE = value; } }

        public decimal AverageVolume
        { get { return _AVG_VOLUME; } set { _AVG_VOLUME = value; } }

        public decimal MarketCap
        { get { return _MARKET_CAP; } set { _MARKET_CAP = value; } }

        public decimal TotalDebt
        { get { return _TOTAL_DEBT; } set { _TOTAL_DEBT = value; } }

        public decimal CurrentSharesShort
        { get { return _SHORT_RATIO_CURRENT; } set { _SHORT_RATIO_CURRENT = value; } }

        public decimal LastMonthSharesShort
        { get { return _SHORT_RATIO_LAST_MONTH; } set { _SHORT_RATIO_LAST_MONTH = value; } }

        public decimal FiftyMovingAverage
        { get { return _50_MA; } set { _50_MA = value; } }

        public decimal TwoHundredMovingAverage
        { get { return _200_MA; } set { _200_MA = value; } }

        public decimal DividendYieldPercent
        { get { return _DIV_YIELD_PERCENT; } set { _DIV_YIELD_PERCENT = value; } }

        public decimal OperatingMargin
        { get { return _OPERATING_MARGIN; } set { _OPERATING_MARGIN = value; } }

        public decimal ProfitMargin
        { get { return _PROFIT_MARGIN; } set { _PROFIT_MARGIN = value; } }

        public decimal TrailingPE
        { get { return _TRAILING_PE; } set { _TRAILING_PE = value; } }

        public decimal ForwardPE
        { get { return _FORWARD_PE; } set { _FORWARD_PE = value; } }

        public decimal PegRatio
        { get { return _PEG_RATIO; } set { _PEG_RATIO = value; } }

        public decimal EnterpriseValue
        { get { return _ENTERPRISE_VALUE; } set { _ENTERPRISE_VALUE = value; } }

        public decimal PriceToSales
        { get { return _PRICE_TO_SALES; } set { _PRICE_TO_SALES = value; } }

        public decimal PriceToBook
        { get { return _PRICE_TO_BOOK; } set { _PRICE_TO_BOOK = value; } }

        public decimal EvToRevenue
        { get { return _EV_REVENUE; } set { _EV_REVENUE = value; } }

        public decimal EvToEbitda
        { get { return _EV_EBITDA; } set { _EV_EBITDA = value; } }

        public decimal QuarterlyRevenueGrowth
        { get { return _Q_REV_GROWTH; } set { _Q_REV_GROWTH = value; } }

        public decimal Ebitda
        { get { return _EBITDA; } set { _EBITDA = value; } }

        public decimal NetIncomeToCommonShares
        { get { return _NI_COMMON_SHARES; } set { _NI_COMMON_SHARES = value; } }

        public decimal QuarterlyEarningsGrowth
        { get { return _Q_EARNINGS_GROWTH; } set { _Q_EARNINGS_GROWTH = value; } }

        public decimal TotalCash
        { get { return _TOTAL_CASH; } set { _TOTAL_CASH = value; } }

        public decimal CurrentRatio
        { get { return _CURRENT_RATIO; } set { _CURRENT_RATIO = value; } }

        public decimal OperatingCashFlow
        { get { return _OP_CF; } set { _OP_CF = value; } }

        public decimal LeveredCashFlow
        { get { return _LEVERED_CF; } set { _LEVERED_CF = value; } }



        public CompanyFinancialStatistics(string symbol, DateTime date)
        {
            this._SYMBOL = symbol;
            this._DATE = date;
            Inititalize();
        }

        public CompanyFinancialStatistics()
        { }

        private void Inititalize()
        {
            this._200_MA = 0;
            this._50_MA = 0;
            this._AVG_VOLUME = 0;
            this._BOOK_VALUE_PER_SHARE = 0;
            this._DEBT_TO_EQUITY = 0;
            this._DIV_YIELD_PERCENT = 0;
            this._EPS = 0;
            this._MARKET_CAP = 0;
            this._OPERATING_MARGIN = 0;
            this._PROFIT_MARGIN = 0;
            this._REVENUE_PER_SHARE = 0;
            this._ROA = 0;
            this._ROE = 0;
            this._STOCK_PRICE = 0;
            this._TOTAL_DEBT = 0;
            this._YEAR_HIGH = 0;
            this._YEAR_LOW = 0;
        }
    }
}
