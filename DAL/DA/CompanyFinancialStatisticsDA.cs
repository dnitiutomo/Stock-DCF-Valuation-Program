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
    public class CompanyFinancialStatisticsDA : AbstractQueryBuilder, ICompanyFinancialStatisticsDA
    {
        private static CompanyFinancialStatisticsDA instance;

        #region CONSTRUCTORS

        public CompanyFinancialStatisticsDA()
            : base() 
        {
            _TABLE_NAME = "yahoo_cfd.financial_statistics";
            _KEY_COLS = new string[] { "SYMBOL", "DATE" };
            _NON_KEY_COLS = new string[] { "BOOK_VALUE_PER_SHARE", "STOCK_PRICE", "ROE", "ROA", "DEBT_TO_EQUITY", 
            "YEAR_HIGH", "YEAR_LOW", "EPS", "REVENUE_PER_SHARE", "AVG_VOLUME", "MARKET_CAP", "TOTAL_DEBT",
            "SHARES_SHORT_CURRENT", "SHARE_SHORT_LAST_MONTH", "FIFTY_MA", "TWO_HUNDRED_MA", "DIV_YIELD", "OP_MARGIN",
            "PROFIT_MARGIN", "TRAILING_PE", "FORWARD_PE", "PEG_RATIO", "ENTERPRISE_VALUE", "PRICE_TO_SALES", "PRICE_TO_BOOK",
            "EV_REVENUE", "EV_EBITDA","Q_REV_GROWTH","Q_EARNINGS_GROWTH","EBITDA", "NI_COMMON_SHARES", "TOTAL_CASH", "CURRENT_RATIO",
            "OP_CASH_FLOW","LEVERED_CASH_FLOW"};
        }

        public static CompanyFinancialStatisticsDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyFinancialStatisticsDA();
                }
                return instance;
            }
        }

        #endregion

        public CompanyFinancialStatistics GetCompanyFinancialStatistics (string tickerSymbol, DateTime date)
        {
            CompanyFinancialStatistics bs = null;
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@DATE", MySqlDbType.DateTime)
            };

            parms[0].Value = tickerSymbol;
            parms[1].Value = date;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SelectEntryFromSQLStatement(), parms))
            {
                if (rdr.Read())
                {
                    bs = ConvertReaderToCompanyFinancialStatisticsObject(rdr);
                }
            }

            return bs;
        }

        #region UPDATE METHODS

        public int CountCompanyFinancialStatistics(string tickerSymbol, DateTime date)
        {
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@DATE", MySqlDbType.DateTime)
            };
            parms[0].Value = tickerSymbol;
            parms[1].Value = date;

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

        public void InsertCompanyFinancialStatistics(CompanyFinancialStatistics inc)
        {
            MySqlParameter[] parms = GetCompanyFinancialStatisticsParameters();
            SetIncomeStatementParameters(inc, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, InsertSQLStatement(), parms);
            }
        }

        public void UpdateCompanyFinancialStatistics(CompanyFinancialStatistics inc)
        {
            MySqlParameter[] parms = GetCompanyFinancialStatisticsParameters();
            SetIncomeStatementParameters(inc, parms);

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, UpdateSQLStatement(), parms);
            }
        }

        #endregion

        #region HELPER METHODS

        private CompanyFinancialStatistics ConvertReaderToCompanyFinancialStatisticsObject(MySqlDataReader rdr)
        {
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

            CompanyFinancialStatistics comp = new CompanyFinancialStatistics();
            comp.Symbol = MySqlHelper.ConvertReaderToString(rdr, sqlParams[i++]);
            comp.Date = MySqlHelper.ConvertReaderToDateTime(rdr, sqlParams[i++]);
            comp.BookValuePerShare = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.StockPrice = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.ReturnOnEquity = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.ReturnOnAssets = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.DebtToEquity = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.YearHigh = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.YearLow = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.Eps = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.RevenuePerShare = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.AverageVolume = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.MarketCap = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.TotalDebt = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.CurrentSharesShort = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.LastMonthSharesShort = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.FiftyMovingAverage = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.TwoHundredMovingAverage = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.DividendYieldPercent = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.OperatingMargin = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.ProfitMargin = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.TrailingPE = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.ForwardPE = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.PegRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.EnterpriseValue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.PriceToSales = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.PriceToBook = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.EvToRevenue = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.EvToEbitda = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.QuarterlyRevenueGrowth = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.QuarterlyEarningsGrowth = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.Ebitda = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.NetIncomeToCommonShares = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.TotalCash = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.CurrentRatio = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.OperatingCashFlow = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            comp.LeveredCashFlow = MySqlHelper.ConvertReaderToDecimal(rdr, sqlParams[i++]);
            
            return comp;
        }

        private static MySqlParameter[] GetCompanyFinancialStatisticsParameters()
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
											new MySqlParameter("@" + sqlParams[i++], MySqlDbType.Date),
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

        private static void SetIncomeStatementParameters(CompanyFinancialStatistics comp, MySqlParameter[] parms)
        {
            int i = 0;
            parms[i++].Value = comp.Symbol;
            parms[i++].Value = comp.Date;
            parms[i++].Value = comp.BookValuePerShare;
            parms[i++].Value = comp.StockPrice;
            parms[i++].Value = comp.ReturnOnEquity;
            parms[i++].Value = comp.ReturnOnAssets;
            parms[i++].Value = comp.DebtToEquity;
            parms[i++].Value = comp.YearHigh;
            parms[i++].Value = comp.YearLow;
            parms[i++].Value = comp.Eps;
            parms[i++].Value = comp.RevenuePerShare;
            parms[i++].Value = comp.AverageVolume;
            parms[i++].Value = comp.MarketCap;
            parms[i++].Value = comp.TotalDebt;
            parms[i++].Value = comp.CurrentSharesShort;
            parms[i++].Value = comp.LastMonthSharesShort;
            parms[i++].Value = comp.FiftyMovingAverage;
            parms[i++].Value = comp.TwoHundredMovingAverage;
            parms[i++].Value = comp.DividendYieldPercent;
            parms[i++].Value = comp.OperatingMargin;
            parms[i++].Value = comp.ProfitMargin;
            parms[i++].Value = comp.TrailingPE;
            parms[i++].Value = comp.ForwardPE;
            parms[i++].Value = comp.PegRatio;
            parms[i++].Value = comp.EnterpriseValue;
            parms[i++].Value = comp.PriceToSales;
            parms[i++].Value = comp.PriceToBook;
            parms[i++].Value = comp.EvToRevenue;
            parms[i++].Value = comp.EvToEbitda;
            parms[i++].Value = comp.QuarterlyRevenueGrowth;
            parms[i++].Value = comp.QuarterlyEarningsGrowth;
            parms[i++].Value = comp.Ebitda;
            parms[i++].Value = comp.NetIncomeToCommonShares;
            parms[i++].Value = comp.TotalCash;
            parms[i++].Value = comp.CurrentRatio;
            parms[i++].Value = comp.OperatingCashFlow;
            parms[i++].Value = comp.LeveredCashFlow;
        }

        #endregion
    }
}
