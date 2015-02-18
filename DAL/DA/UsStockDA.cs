using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.DA
{
    public class UsStockDA : AbstractQueryBuilder, IUsStocksDA
    {
        private static UsStockDA instance;
        private const string SQL_SELECT_COMP_YEARS = "SELECT * FROM quandl.us_stocks WHERE SYMBOL = @SYMBOL AND (YEAR >= @FROMYEAR AND YEAR <= @TOYEAR)";
        private const string SQL_SELECT_YEARS = "SELECT * FROM quandl.us_stocks WHERE YEAR = @YEAR";
        private const string SQL_IS_REV_POPULATED = "SELECT COUNT(*) FROM QUANDL.US_STOCKS WHERE SYMBOL = @SYMBOL AND TRAILING_REVENUE <> 0";
        private const string SQL_GET_COMPANY_LIST = "SELECT DISTINCT(SYMBOL) FROM QUANDL.US_STOCKS";

        #region CONSTRUCTORS

        public UsStockDA()
            : base() 
        {
            _TABLE_NAME = "quandl.us_stocks";
            _KEY_COLS = new string[] { "SYMBOL", "YEAR" };
            _NON_KEY_COLS = new string[] { "SHARES_OUTSTANDING", "BETA_3YR", "STOCK_PRICE_STD_DEV", "BOOK_DEBT_CAPITAL_RATIO", "EQUITY", 
            "ASSETS", "CAPEX", "CASH", "CASH_PERCENTAGE_OF_FIRM_VALUE", "CASH_PERCENTAGE_OF_REVENUE", "CASH_PERCENTAGE_OF_ASSETS", "CHANGE_NON_CASH_WORKING_CAPITAL",
            "MARKET_CORRELATION", "CURRENT_PE_RATIO", "DEPRECIATION", "DIVIDEND_YIELD", "DIVIDENDS", "EBIT",
            "EBIT_PREVIOUS_PERIOD", "EBITDA", "EFFECTIVE_TAX_RATE","EFFECTIVE_TAX_RATE_ON_INCOME", "ENTERPRISE_VALUE", "EV_INVESTED_CAPITAL_RATIO", "EV_TRAILING_SALES_RATIO","EV_EBIT_RATIO", "EV_EBITDA_RATIO",
            "EV_SALES_RATIO", "EPS_EXPECTED_GROWTH","REVENUE_EXPECTED_GROWTH","FREE_CASH_FLOW","FIRM_VALUE", "FIXED_TO_TOTAL_ASSET_RATIO", "FORWARD_EPS", "FORWARD_PE_RATIO",
            "EPS_GROWTH","REVENUE_GROWTH_PREV_YEAR", "HI_LO_RISK", "INSIDER_HOLDINGS", "INSTITUTIONAL_HOLDINGS", "INTANGIBLE_ASSETS_TOTAL_ASSETS_RATIO", "INVESTED_CAPITAL", "MARKET_CAPITILIZATION",
            "DEBT_EQUITY_RATIO","DEBT_TO_CAPITAL" ,"NET_INCOME", "NET_MARGIN", "NON_CASH_WORKING_CAPITAL", "NON_CASH_WORKING_CAPITAL_REVENUE_RATIO", "PAYOUT_RATIO",
            "PRICE_BOOK_RATIO", "PE_GROWTH_RATIO", "OPERATING_MARGIN", "PRICE_SALES_RATIO", "REINVESTMENT_AMOUNT", "REINVESTMENT_RATE", "REVENUES",
            "RETURN_ON_CAPITAL", "RETURN_ON_EQUITY", "SGA_EXPENSE", "STOCK_PRICE","TOTAL_DEBT","TRADING_VOLUME", "TTM_REVENUES", "TRAILING_NET_INCOME",
            "TRAILING_PE_RATIO", "TRAILING_REVENUE", "BETA_VALUE_LINE", "EV_BOOK_RATIO"
            };
        }

        public static UsStockDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsStockDA();
                }
                return instance;
            }
        }

        #endregion
        public UsStock GetUsStock(string tickerSymbol, int year)
        {
            UsStock stock = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@YEAR", MySqlDbType.Int16)
            };


            parms[0].Value = tickerSymbol;
            parms[1].Value = year;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SelectEntryFromSQLStatement(), parms))
            {
                if (rdr.Read())
                {
                    stock = ConvertReaderToUsStockObject(rdr);
                }
            }

            return stock;
        }
        public UsStockCollection GetUsStocks(string tickerSymbol, int fromYear, int toYear)
        {
            UsStockCollection stocks = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@FROMYEAR", MySqlDbType.Int16),
                new MySqlParameter("@TOYEAR", MySqlDbType.Int16)
            };

            parms[0].Value = tickerSymbol;
            parms[1].Value = fromYear;
            parms[2].Value = toYear;

            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_COMP_YEARS, parms))
            {
                if (rdr.Read())
                {
                    //If there is one result
                    stocks = new UsStockCollection();
                    //Scroll through the results
                    do
                    {
                        stocks.Add(ConvertReaderToUsStockObject(rdr));
                    }
                    while (rdr.Read());
                }
            }

            return stocks;
        }

        public UsStockCollection GetUsStocks(int year)
        {
            UsStockCollection stocks = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@YEAR", MySqlDbType.Int16)
            };

            parms[0].Value = year;

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {

                using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_YEARS, parms))
                {
                    if (rdr.Read())
                    {
                        //If there is one result
                        stocks = new UsStockCollection();
                        //Scroll through the results
                        do
                        {
                            stocks.Add(ConvertReaderToUsStockObject(rdr));
                        }
                        while (rdr.Read());
                    }
                }
            }

            return stocks;
        }

        public int CountUsStock(string tickerSymbol, int year)
        {
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@YEAR", MySqlDbType.Int32)
            };
            parms[0].Value = tickerSymbol;
            parms[1].Value = year;

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                try
                {
                    conn.Open();
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, CountEntrySQLStatement(), parms));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public bool IsRevenuePopulated(string symbol)
        {
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar)
            };
            parms[0].Value = symbol;

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_IS_REV_POPULATED, parms));
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public List<string> GetCompanyList()
        {
            List<string> companies = new List<string>();

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_GET_COMPANY_LIST))
            {
                if (rdr.Read())
                {
                    do
                    {
                        companies.Add(MySqlHelper.ConvertReaderToString(rdr, "SYMBOL"));
                    }
                    while (rdr.Read());
                }
            }

            return companies;
        }

        public void InsertUsStock(UsStock stock)
        {
            MySqlParameter[] parms = GetUsStockParameters();
            SetUsStockParameters(stock, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, InsertSQLStatement(), parms);
            }
        }

        public void UpdateUsStock(UsStock stock)
        {
            MySqlParameter[] parms = GetUsStockParameters();
            SetUsStockParameters(stock, parms);

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, UpdateSQLStatement(), parms);
            }
        }


        #region HELPER METHODS

        private UsStock ConvertReaderToUsStockObject(MySqlDataReader rdr)
        {
            UsStock stock = new UsStock();
            List<string> sqlParams = new List<string>();
            foreach (string s in _KEY_COLS)
            {
                sqlParams.Add(s);
            }
            foreach (string s in _NON_KEY_COLS)
            {
                sqlParams.Add(s);
            }

            int i = 0;

            stock.Symbol = MySqlHelper.ConvertReaderToString(rdr, sqlParams[i++]);
            stock.Year = MySqlHelper.ConvertReaderToInt(rdr, sqlParams[i++]);
            stock.SharesOutstanding = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Beta3Yr = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.StdDev3YrStockPrice = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.BookDebttoCapitalRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EquityBookValue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.AssetBookValue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.CapEx = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Cash = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.CashPercentageOfFirmValue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.CashPercentageOfRevenue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.CashPercentageOfAssets = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ChangeNonCashWorkingCapital = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.MarketCorrelation = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.CurrentPeRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Depreciation = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.DividendYield = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Dividends = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Ebit = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EbitPreviousPeriod = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Ebitda = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EffectiveTaxRate = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EffectiveTaxRateOnIncome = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            
            stock.EnterpriseValue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EvToInvestedCapitalRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EvToTrailingSalesRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);

            stock.EvToEbitRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EvToEbitdaRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EvToSalesRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EpsExpectedGrowth = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.RevenueExpectedGrowth = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.FreeCashFlow = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.FirmValue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.FixedToTotalAssetRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ForwardEps = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ForwardPeRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EpsGrowth = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);

            stock.RevenueGrowthPrevYear = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.HiLoRisk = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.InsiderHoldings = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.InstitutionalHoldings = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.IntangibleAssetsToTotalAssetsRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.InvestedCapital = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.MarketCap = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.DebtToEquityRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.DebtToCapitalRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            
            stock.NetIncome = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.NetMargin = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.NonCashWorkingCapital = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.NonCashWorkingCapitalToRevenueRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.PayoutRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.PriceToBookRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.PeToGrowthRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.OperatingMargin = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.PriceToSalesRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ReinvestmentAmount = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ReinvestmentRate = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.Revenues = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ReturnOnCapital = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.ReturnOnEquity = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.SgaExpense = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.StockPrice = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);

            stock.TotalDebt = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.TradingVolume = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            
            stock.TtmRevenues = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.TrailingNetIncome = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.TrailingPeRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.TrailingRevenues = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.BetaValueLine = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            stock.EvToBookRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);

            return stock;
        }
        private static MySqlParameter[] GetUsStockParameters()
        {
            MySqlParameter[] parms;
            List<string> sqlParams = new List<string>();

            foreach (string s in _KEY_COLS)
            {
                sqlParams.Add(s);
            }
            foreach (string s in _NON_KEY_COLS)
            {
                sqlParams.Add(s);
            }

            int i = 0;

            parms = new MySqlParameter[] {
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.VarChar),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Int16),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
                                            new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
                                            new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
                                            new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
                                            new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
                                            new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
                                            new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal),
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Decimal)
											};

            return parms;
        }

        private static void SetUsStockParameters(UsStock comp, MySqlParameter[] parms)
        {
            int i = 0;
            parms[i++].Value = comp.Symbol;
            parms[i++].Value = comp.Year;
            parms[i++].Value = comp.SharesOutstanding;
            parms[i++].Value = comp.Beta3Yr;
            parms[i++].Value = comp.StdDev3YrStockPrice;
            parms[i++].Value = comp.BookDebttoCapitalRatio;
            parms[i++].Value = comp.EquityBookValue;
            parms[i++].Value = comp.AssetBookValue;
            parms[i++].Value = comp.CapEx;
            parms[i++].Value = comp.Cash;
            parms[i++].Value = comp.CashPercentageOfFirmValue;
            parms[i++].Value = comp.CashPercentageOfRevenue;
            parms[i++].Value = comp.CashPercentageOfAssets;
            parms[i++].Value = comp.ChangeNonCashWorkingCapital;
            parms[i++].Value = comp.MarketCorrelation;
            parms[i++].Value = comp.CurrentPeRatio;
            parms[i++].Value = comp.Depreciation;
            parms[i++].Value = comp.DividendYield;
            parms[i++].Value = comp.Dividends;
            parms[i++].Value = comp.Ebit;
            parms[i++].Value = comp.EbitPreviousPeriod;
            parms[i++].Value = comp.Ebitda;
            parms[i++].Value = comp.EffectiveTaxRate;
            parms[i++].Value = comp.EffectiveTaxRateOnIncome;
            parms[i++].Value = comp.EnterpriseValue;
            parms[i++].Value = comp.EvToInvestedCapitalRatio;
            parms[i++].Value = comp.EvToTrailingSalesRatio;
            parms[i++].Value = comp.EvToEbitRatio;
            parms[i++].Value = comp.EvToEbitdaRatio;
            parms[i++].Value = comp.EvToSalesRatio;
            parms[i++].Value = comp.EpsExpectedGrowth;
            parms[i++].Value = comp.RevenueExpectedGrowth;
            parms[i++].Value = comp.FreeCashFlow;
            parms[i++].Value = comp.FirmValue;
            parms[i++].Value = comp.FixedToTotalAssetRatio;
            parms[i++].Value = comp.ForwardEps;
            parms[i++].Value = comp.ForwardPeRatio;
            parms[i++].Value = comp.EpsGrowth;
            parms[i++].Value = comp.RevenueGrowthPrevYear;

            parms[i++].Value = comp.HiLoRisk;
            parms[i++].Value = comp.InsiderHoldings;
            parms[i++].Value = comp.InstitutionalHoldings;
            parms[i++].Value = comp.IntangibleAssetsToTotalAssetsRatio;
            parms[i++].Value = comp.InvestedCapital;
            parms[i++].Value = comp.MarketCap;
            parms[i++].Value = comp.DebtToEquityRatio;
            parms[i++].Value = comp.DebtToCapitalRatio;
            parms[i++].Value = comp.NetIncome;
            parms[i++].Value = comp.NetMargin;
            parms[i++].Value = comp.NonCashWorkingCapital;
            parms[i++].Value = comp.NonCashWorkingCapitalToRevenueRatio;
            parms[i++].Value = comp.PayoutRatio;
            parms[i++].Value = comp.PriceToBookRatio;
            parms[i++].Value = comp.PeToGrowthRatio;
            parms[i++].Value = comp.OperatingMargin;
            parms[i++].Value = comp.PriceToSalesRatio;
            parms[i++].Value = comp.ReinvestmentAmount;
            parms[i++].Value = comp.ReinvestmentRate;
            parms[i++].Value = comp.Revenues;
            parms[i++].Value = comp.ReturnOnCapital;
            parms[i++].Value = comp.ReturnOnEquity;
            parms[i++].Value = comp.SgaExpense;
            parms[i++].Value = comp.StockPrice;
            parms[i++].Value = comp.TotalDebt;
            parms[i++].Value = comp.TradingVolume;
            parms[i++].Value = comp.TtmRevenues;
            parms[i++].Value = comp.TrailingNetIncome;
            parms[i++].Value = comp.TrailingPeRatio;
            parms[i++].Value = comp.TrailingRevenues;
            parms[i++].Value = comp.BetaValueLine;
            parms[i++].Value = comp.EvToBookRatio;
        }

        #endregion

    }
}
