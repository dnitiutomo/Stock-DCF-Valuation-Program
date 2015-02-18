using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.BOL
{
    public class CompanyFinancialRanking
    {
        public CompanyFinancialRanking(string symbol)
        {
            this._SYMBOL = symbol;
        }

        public CompanyFinancialRanking()
        { }

        private string _SYMBOL;
        private decimal _BOOK_VALUE_PER_SHARE;
        private decimal _ROE;
        private decimal _ROA;
        private decimal _DEBT_TO_EQUITY;
        private decimal _ROIC;
        private decimal _EPS;
        private decimal _REVENUE_PER_SHARE;
        private decimal _AVG_VOLUME;
        private decimal _SHORT_RATIO_CURRENT;
        private decimal _SHORT_RATIO_LAST_MONTH;
        private decimal _DIV_YIELD_PERCENT;
        private decimal _OPERATING_MARGIN;
        private decimal _PROFIT_MARGIN;
        private decimal _TRAILING_PE;
        private decimal _FORWARD_PE;
        private decimal _PEG_RATIO;
        private decimal _PRICE_TO_SALES;
        private decimal _PRICE_TO_BOOK;
        private decimal _EV_REVENUE;
        private decimal _EV_EBITDA;
        private decimal _Q_REV_GROWTH;
        private decimal _Q_EARNINGS_GROWTH;
        private decimal _CURRENT_RATIO;
        private decimal _OP_CF;
        private decimal _LEVERED_CF;
        private decimal _NOP_AVG_RANK;
        private decimal _NOP_DECAY_RANK;
        private decimal _NOP_NO_RANK;
        private decimal _NI_AVG_RANK;
        private decimal _NI_DECAY_RANK;
        private decimal _NI_NO_RANK;

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public decimal BookValuePerShareDiffRanking
        { get { return _BOOK_VALUE_PER_SHARE; } set { _BOOK_VALUE_PER_SHARE = value; } }

        public decimal ReturnOnEquityRanking
        { get { return _ROE; } set { _ROE = value; } }

        public decimal ReturnOnAssetsRanking
        { get { return _ROA; } set { _ROA = value; } }

        public decimal DebtToEquityRanking
        { get { return _DEBT_TO_EQUITY; } set { _DEBT_TO_EQUITY = value; } }

        public decimal RoicRanking
        { get { return _ROIC; } set { _ROIC = value; } }

        public decimal EpsRanking
        { get { return _EPS; } set { _EPS = value; } }

        public decimal RevenuePerShareRanking
        { get { return _REVENUE_PER_SHARE; } set { _REVENUE_PER_SHARE = value; } }

        public decimal AverageVolumeRanking
        { get { return _AVG_VOLUME; } set { _AVG_VOLUME = value; } }

        public decimal CurrentSharesShortRanking
        { get { return _SHORT_RATIO_CURRENT; } set { _SHORT_RATIO_CURRENT = value; } }

        public decimal LastMonthSharesShortRanking
        { get { return _SHORT_RATIO_LAST_MONTH; } set { _SHORT_RATIO_LAST_MONTH = value; } }

        public decimal DividendYieldPercentRanking
        { get { return _DIV_YIELD_PERCENT; } set { _DIV_YIELD_PERCENT = value; } }

        public decimal OperatingMarginRanking
        { get { return _OPERATING_MARGIN; } set { _OPERATING_MARGIN = value; } }

        public decimal ProfitMarginRanking
        { get { return _PROFIT_MARGIN; } set { _PROFIT_MARGIN = value; } }

        public decimal TrailingPERanking
        { get { return _TRAILING_PE; } set { _TRAILING_PE = value; } }

        public decimal ForwardPERanking
        { get { return _FORWARD_PE; } set { _FORWARD_PE = value; } }

        public decimal PegRatioRanking
        { get { return _PEG_RATIO; } set { _PEG_RATIO = value; } }

        public decimal PriceToSalesRanking
        { get { return _PRICE_TO_SALES; } set { _PRICE_TO_SALES = value; } }

        public decimal PriceToBookRanking
        { get { return _PRICE_TO_BOOK; } set { _PRICE_TO_BOOK = value; } }

        public decimal EvToRevenueRanking
        { get { return _EV_REVENUE; } set { _EV_REVENUE = value; } }

        public decimal EvToEbitdaRanking
        { get { return _EV_EBITDA; } set { _EV_EBITDA = value; } }

        public decimal QuarterlyRevenueGrowthRanking
        { get { return _Q_REV_GROWTH; } set { _Q_REV_GROWTH = value; } }

        public decimal QuarterlyEarningsGrowthRanking
        { get { return _Q_EARNINGS_GROWTH; } set { _Q_EARNINGS_GROWTH = value; } }

        public decimal CurrentRatioRanking
        { get { return _CURRENT_RATIO; } set { _CURRENT_RATIO = value; } }

        public decimal OperatingCashFlowRanking
        { get { return _OP_CF; } set { _OP_CF = value; } }

        public decimal LeveredCashFlowRanking
        { get { return _LEVERED_CF; } set { _LEVERED_CF = value; } }

        public decimal NopAvgGrowthRanking
        { get { return _NOP_AVG_RANK; } set { _NOP_AVG_RANK = value; } }

        public decimal NopDecayGrowthRanking
        { get { return _NOP_DECAY_RANK; } set { _NOP_DECAY_RANK = value; } }

        public decimal NopNoGrowthRanking
        { get { return _NOP_NO_RANK; } set { _NOP_NO_RANK = value; } }

        public decimal NiAvgGrowthRanking
        { get { return _NI_AVG_RANK; } set { _NI_AVG_RANK = value; } }

        public decimal NiDecayGrowthRanking
        { get { return _NI_DECAY_RANK; } set { _NI_DECAY_RANK = value; } }

        public decimal NiNoGrowthRanking
        { get { return _NI_NO_RANK; } set { _NI_NO_RANK = value; } }
    }
}
