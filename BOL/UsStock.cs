using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.BOL
{
    public class UsStock
    {
        #region PRIVATE MEMBERS

        private string _SYMBOL;
	    public string Symbol
	    {
		    get { return _SYMBOL;}
		    set { _SYMBOL = value;}
	    }
	
        private int _YEAR;
        public int Year
	    {
		    get { return _YEAR;}
		    set { _YEAR = value;}
	    }

        private decimal _SHARES_OUTSTANDING;
        public decimal SharesOutstanding
	    {
		    get { return _SHARES_OUTSTANDING;}
		    set { _SHARES_OUTSTANDING = value;}
	    }

        private decimal _BETA_3YR;
        public decimal Beta3Yr
	    {
		    get { return _BETA_3YR;}
		    set { _BETA_3YR = value;}
	    }

        private decimal _STOCK_PRICE_STD_DEV;
        public decimal StdDev3YrStockPrice
	    {
		    get { return _STOCK_PRICE_STD_DEV;}
		    set { _STOCK_PRICE_STD_DEV = value;}
	    }

        private decimal _BOOK_DEBT_CAPITAL_RATIO;
        public decimal BookDebttoCapitalRatio
	    {
            get { return _BOOK_DEBT_CAPITAL_RATIO; }
            set { _BOOK_DEBT_CAPITAL_RATIO = value; }
	    }

        private decimal _EQUITY;
        public decimal EquityBookValue
	    {
		    get { return _EQUITY;}
		    set { _EQUITY = value;}
	    }

        private decimal _ASSETS;
        public decimal AssetBookValue
	    {
		    get { return _ASSETS;}
		    set { _ASSETS = value;}
	    }

        private decimal _CAPEX;
        public decimal CapEx
	    {
		    get { return _CAPEX;}
		    set { _CAPEX = value;}
	    }

        private decimal _CASH;
        public decimal Cash
	    {
		    get { return _CASH;}
		    set { _CASH = value;}
	    }

        private decimal _CASH_PERCENTAGE_OF_FIRM_VALUE;
        public decimal CashPercentageOfFirmValue
	    {
		    get { return _CASH_PERCENTAGE_OF_FIRM_VALUE;}
		    set { _CASH_PERCENTAGE_OF_FIRM_VALUE = value;}
	    }

        private decimal _CASH_PERCENTAGE_OF_REVENUE;
        public decimal CashPercentageOfRevenue
	    {
		    get { return _CASH_PERCENTAGE_OF_REVENUE;}
		    set { _CASH_PERCENTAGE_OF_REVENUE = value;}
	    }

        private decimal _CASH_PERCENTAGE_OF_ASSETS;
        public decimal CashPercentageOfAssets
	    {
		    get { return _CASH_PERCENTAGE_OF_ASSETS;}
		    set { _CASH_PERCENTAGE_OF_ASSETS = value;}
	    }

        private decimal _CHANGE_NON_CASH_WORKING_CAPITAL;
        public decimal ChangeNonCashWorkingCapital
	    {
		    get { return _CHANGE_NON_CASH_WORKING_CAPITAL;}
		    set { _CHANGE_NON_CASH_WORKING_CAPITAL = value;}
	    }

        private decimal _MARKET_CORRELATION;
        public decimal MarketCorrelation
	    {
		    get { return _MARKET_CORRELATION;}
		    set { _MARKET_CORRELATION = value;}
	    }

        private decimal _CURRENT_PE_RATIO;
        public decimal CurrentPeRatio
	    {
		    get { return _CURRENT_PE_RATIO;}
		    set { _CURRENT_PE_RATIO = value;}
	    }

        private decimal _DEPRECIATION;
        public decimal Depreciation
	    {
		    get { return _DEPRECIATION;}
		    set { _DEPRECIATION = value;}
	    }

        private decimal _DIVIDEND_YIELD;
        public decimal DividendYield
	    {
		    get { return _DIVIDEND_YIELD;}
		    set { _DIVIDEND_YIELD = value;}
	    }

        private decimal _DIVIDENDS;
        public decimal Dividends
	    {
		    get { return _DIVIDENDS;}
		    set { _DIVIDENDS = value;}
	    }

        private decimal _EBIT;
        public decimal Ebit
	    {
		    get { return _EBIT;}
		    set { _EBIT = value;}
	    }

        private decimal _EBIT_PREVIOUS_PERIOD;
        public decimal EbitPreviousPeriod
	    {
		    get { return _EBIT_PREVIOUS_PERIOD;}
		    set { _EBIT_PREVIOUS_PERIOD = value;}
	    }

        private decimal _EBITDA;
        public decimal Ebitda
	    {
		    get { return _EBITDA;}
		    set { _EBITDA = value;}
	    }

        private decimal _EFFECTIVE_TAX_RATE;
        public decimal EffectiveTaxRate
	    {
		    get { return _EFFECTIVE_TAX_RATE;}
		    set { _EFFECTIVE_TAX_RATE = value;}
	    }

        private decimal _EFFECTIVE_TAX_RATE_ON_INCOME;
        public decimal EffectiveTaxRateOnIncome
        {
            get { return _EFFECTIVE_TAX_RATE_ON_INCOME; }
            set { _EFFECTIVE_TAX_RATE_ON_INCOME = value; }
        }

        private decimal _ENTERPRISE_VALUE;
        public decimal EnterpriseValue
	    {
		    get { return _ENTERPRISE_VALUE;}
		    set { _ENTERPRISE_VALUE = value;}
	    }

        private decimal _EV_INVESTED_CAPITAL_RATIO;
        public decimal EvToInvestedCapitalRatio
	    {
		    get { return _EV_INVESTED_CAPITAL_RATIO;}
		    set { _EV_INVESTED_CAPITAL_RATIO = value;}
	    }

        private decimal _EV_TRAILING_SALES_RATIO;
        public decimal EvToTrailingSalesRatio
	    {
		    get { return _EV_TRAILING_SALES_RATIO;}
		    set { _EV_TRAILING_SALES_RATIO = value;}
	    }

        private decimal _EV_EBIT_RATIO;
        public decimal EvToEbitRatio
        {
            get { return _EV_EBIT_RATIO; }
            set { _EV_EBIT_RATIO = value; }
        }

        private decimal _EV_EBITDA_RATIO;
        public decimal EvToEbitdaRatio
	    {
		    get { return _EV_EBITDA_RATIO;}
		    set { _EV_EBITDA_RATIO = value;}
	    }

        private decimal _EV_SALES_RATIO;
        public decimal EvToSalesRatio
	    {
		    get { return _EV_SALES_RATIO;}
		    set { _EV_SALES_RATIO = value;}
	    }

        private decimal _EPS_EXPECTED_GROWTH;
        public decimal EpsExpectedGrowth
	    {
		    get { return _EPS_EXPECTED_GROWTH;}
		    set { _EPS_EXPECTED_GROWTH = value;}
	    }

        private decimal _REVENUE_EXPECTED_GROWTH;
        public decimal RevenueExpectedGrowth
	    {
		    get { return _REVENUE_EXPECTED_GROWTH;}
		    set { _REVENUE_EXPECTED_GROWTH = value;}
	    }

        private decimal _FREE_CASH_FLOW;
        public decimal FreeCashFlow
	    {
		    get { return _FREE_CASH_FLOW;}
		    set { _FREE_CASH_FLOW = value;}
	    }

        private decimal _FIRM_VALUE;
        public decimal FirmValue
	    {
		    get { return _FIRM_VALUE;}
		    set { _FIRM_VALUE = value;}
	    }

        private decimal _FIXED_TO_TOTAL_ASSET_RATIO;
        public decimal FixedToTotalAssetRatio
	    {
		    get { return _FIXED_TO_TOTAL_ASSET_RATIO;}
		    set { _FIXED_TO_TOTAL_ASSET_RATIO = value;}
	    }

        private decimal _FORWARD_EPS;
        public decimal ForwardEps
	    {
		    get { return _FORWARD_EPS;}
		    set { _FORWARD_EPS = value;}
	    }

        private decimal _FORWARD_PE_RATIO;
        public decimal ForwardPeRatio
	    {
		    get { return _FORWARD_PE_RATIO;}
		    set { _FORWARD_PE_RATIO = value;}
	    }

        private decimal _EPS_GROWTH;
        public decimal EpsGrowth
	    {
		    get { return _EPS_GROWTH;}
		    set { _EPS_GROWTH = value;}
	    }

        private decimal _REVENUE_GROWTH_PREV_YEAR;
        public decimal RevenueGrowthPrevYear
	    {
		    get { return _REVENUE_GROWTH_PREV_YEAR;}
		    set { _REVENUE_GROWTH_PREV_YEAR = value;}
	    }

        private decimal _HI_LO_RISK;
        public decimal HiLoRisk
	    {
		    get { return _HI_LO_RISK;}
		    set { _HI_LO_RISK = value;}
	    }

        private decimal _INSIDER_HOLDINGS;
        public decimal InsiderHoldings
	    {
		    get { return _INSIDER_HOLDINGS;}
		    set { _INSIDER_HOLDINGS = value;}
	    }

        private decimal _INSTITUTIONAL_HOLDINGS;
        public decimal InstitutionalHoldings
	    {
		    get { return _INSTITUTIONAL_HOLDINGS;}
		    set { _INSTITUTIONAL_HOLDINGS = value;}
	    }

        private decimal _INTANGIBLE_ASSETS_TOTAL_ASSETS_RATIO;
        public decimal IntangibleAssetsToTotalAssetsRatio
	    {
		    get { return _INTANGIBLE_ASSETS_TOTAL_ASSETS_RATIO;}
		    set { _INTANGIBLE_ASSETS_TOTAL_ASSETS_RATIO = value;}
	    }

        private decimal _INVESTED_CAPITAL;
        public decimal InvestedCapital
	    {
		    get { return _INVESTED_CAPITAL;}
		    set { _INVESTED_CAPITAL = value;}
	    }

        private decimal _MARKET_CAPITILIZATION;
        public decimal MarketCap
	    {
		    get { return _MARKET_CAPITILIZATION;}
		    set { _MARKET_CAPITILIZATION = value;}
	    }

        private decimal _DEBT_EQUITY_RATIO;
        public decimal DebtToEquityRatio
	    {
		    get { return _DEBT_EQUITY_RATIO;}
		    set { _DEBT_EQUITY_RATIO = value;}
	    }

        private decimal DEBT_TO_CAPITAL_RATIO;
        public decimal DebtToCapitalRatio
        {
            get { return DEBT_TO_CAPITAL_RATIO; }
            set { DEBT_TO_CAPITAL_RATIO = value; }
        }

        private decimal _NET_INCOME;
        public decimal NetIncome
	    {
		    get { return _NET_INCOME;}
		    set { _NET_INCOME = value;}
	    }

        private decimal _NET_MARGIN;
        public decimal NetMargin
	    {
		    get { return _NET_MARGIN;}
		    set { _NET_MARGIN = value;}
	    }

        private decimal _NON_CASH_WORKING_CAPITAL;
        public decimal NonCashWorkingCapital
	    {
		    get { return _NON_CASH_WORKING_CAPITAL;}
		    set { _NON_CASH_WORKING_CAPITAL = value;}
	    }

        private decimal _NON_CASH_WORKING_CAPITAL_REVENUE_RATIO;
        public decimal NonCashWorkingCapitalToRevenueRatio
	    {
		    get { return _NON_CASH_WORKING_CAPITAL_REVENUE_RATIO;}
		    set { _NON_CASH_WORKING_CAPITAL_REVENUE_RATIO = value;}
	    }

        private decimal _PAYOUT_RATIO;
        public decimal PayoutRatio
	    {
		    get { return _PAYOUT_RATIO;}
		    set { _PAYOUT_RATIO = value;}
	    }

        private decimal _PRICE_BOOK_RATIO;
        public decimal PriceToBookRatio
	    {
		    get { return _PRICE_BOOK_RATIO;}
		    set { _PRICE_BOOK_RATIO = value;}
	    }

        private decimal _PE_GROWTH_RATIO;
        public decimal PeToGrowthRatio
	    {
		    get { return _PE_GROWTH_RATIO;}
		    set { _PE_GROWTH_RATIO = value;}
	    }

        private decimal _OPERATING_MARGIN;
        public decimal OperatingMargin
	    {
		    get { return _OPERATING_MARGIN;}
		    set { _OPERATING_MARGIN = value;}
	    }


        private decimal _PRICE_SALES_RATIO;
        public decimal PriceToSalesRatio
	    {
		    get { return _PRICE_SALES_RATIO;}
		    set { _PRICE_SALES_RATIO = value;}
	    }

        private decimal _REINVESTMENT_AMOUNT;
        public decimal ReinvestmentAmount
	    {
		    get { return _REINVESTMENT_AMOUNT;}
		    set { _REINVESTMENT_AMOUNT = value;}
	    }

        private decimal _REINVESTMENT_RATE;
        public decimal ReinvestmentRate
	    {
		    get { return _REINVESTMENT_RATE;}
		    set { _REINVESTMENT_RATE = value;}
	    }

        private decimal _REVENUES;
        public decimal Revenues
	    {
		    get { return _REVENUES;}
		    set { _REVENUES = value;}
	    }

        private decimal _RETURN_ON_CAPITAL;
        public decimal ReturnOnCapital
	    {
		    get { return _RETURN_ON_CAPITAL;}
		    set { _RETURN_ON_CAPITAL = value;}
	    }

        private decimal _RETURN_ON_EQUITY;
        public decimal ReturnOnEquity
	    {
		    get { return _RETURN_ON_EQUITY;}
		    set { _RETURN_ON_EQUITY = value;}
	    }

        private decimal _SGA_EXPENSE;
        public decimal SgaExpense
	    {
		    get { return _SGA_EXPENSE;}
		    set { _SGA_EXPENSE = value;}
	    }

        private decimal _STOCK_PRICE;
        public decimal StockPrice
	    {
		    get { return _STOCK_PRICE;}
		    set { _STOCK_PRICE = value;}
	    }

        private decimal TOTAL_DEBT;
        public decimal TotalDebt
        {
            get { return TOTAL_DEBT; }
            set { TOTAL_DEBT = value; }
        }

        private decimal TRADING_VOLUME;
        public decimal TradingVolume
        {
            get { return TRADING_VOLUME; }
            set { TRADING_VOLUME = value; }
        }

        private decimal _TTM_REVENUES;
        public decimal TtmRevenues
	    {
		    get { return _TTM_REVENUES;}
		    set { _TTM_REVENUES = value;}
	    }

        private decimal _TRAILING_NET_INCOME;
        public decimal TrailingNetIncome
	    {
		    get { return _TRAILING_NET_INCOME;}
		    set { _TRAILING_NET_INCOME = value;}
	    }


        private decimal _TRAILING_PE_RATIO;
        public decimal TrailingPeRatio
	    {
		    get { return _TRAILING_PE_RATIO;}
		    set { _TRAILING_PE_RATIO = value;}
	    }

        private decimal _TRAILING_REVENUE;
        public decimal TrailingRevenues
	    {
		    get { return _TRAILING_REVENUE;}
		    set { _TRAILING_REVENUE = value;}
	    }

        private decimal _BETA_VALUE_LINE;
        public decimal BetaValueLine
	    {
		    get { return _BETA_VALUE_LINE;}
		    set { _BETA_VALUE_LINE = value;}
	    }

        private decimal _EV_BOOK_RATIO;
        public decimal EvToBookRatio
	    {
		    get { return _EV_BOOK_RATIO;}
		    set { _EV_BOOK_RATIO = value;}
	    }

        #endregion

    }
}
