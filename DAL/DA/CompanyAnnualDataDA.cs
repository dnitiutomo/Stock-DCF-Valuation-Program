using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.DA
{
    public class CompanyAnnualDataDA : ICompanyAnnualDataDA
    {
        #region DATA MEMBERS

        private const string SQL_SELECT_COMPANY = "SELECT * FROM annual_company_data WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";
        private const string SQL_SELECT_COUNT_COMPANYANNUAL = "SELECT COUNT(*) FROM annual_company_data WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";

        private const string SQL_INSERT_COMPANYANNUAL =
            @"INSERT INTO annual_company_data
            (SYMBOL,
            YEAR,
            COST_OF_DEBT,
            LEVERED_BETA,
            SHARES_OUTSTANDING,
            DIVIDEND_YIELD)
            VALUES
            (@SYMBOL,
            @YEAR,
            @COST_OF_DEBT,
            @LEVERED_BETA,
            @SHARES_OUTSTANDING,
            @DIVIDEND_YIELD)";

        private const string SQL_UPDATE_COMPANYANNUAL =
            @"UPDATE annual_company_data SET
            COST_OF_DEBT = @COST_OF_DEBT,
            LEVERED_BETA = @LEVERED_BETA,
            SHARES_OUTSTANDING = @SHARES_OUTSTANDING,
            DIVIDEND_YIELD = @DIVIDEND_YIELD
            WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";

        #endregion
        
        #region CONSTRUCTORS

        private static CompanyAnnualDataDA instance;

        public CompanyAnnualDataDA() { }

        public static CompanyAnnualDataDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyAnnualDataDA();
                }
                return instance;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public CompanyAnnualData GetCompanyAnnual(string tickerSymbol, int year)
        {
            CompanyAnnualData cmp = null;
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@YEAR", MySqlDbType.Int16)
            };

            parms[0].Value = tickerSymbol;
            parms[1].Value = year;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_COMPANY, parms))
            {
                if (rdr.Read())
                {
                    cmp = ConvertReaderToCompanyAnnualObject(rdr);
                }
            }

            return cmp;
        }

        public int CountCompanyAnnual(string tickerSymbol, int year)
        {
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@YEAR", MySqlDbType.Int16)
            };
            parms[0].Value = tickerSymbol;
            parms[1].Value = year;

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                try
                {
                    conn.Open();
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_SELECT_COUNT_COMPANYANNUAL, parms));
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion

        #region UPDATE METHODS

        public void InsertCompanyAnnual(CompanyAnnualData comp)
        {
            MySqlParameter[] parms = GetCompanyAnnualParameters();
            SetCompanyAnnualParameters(comp, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_INSERT_COMPANYANNUAL, parms);
            }
        }

        public void UpdateCompanyAnnual(CompanyAnnualData comp)
        {
            MySqlParameter[] parms = GetCompanyAnnualParameters();
            SetCompanyAnnualParameters(comp, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_UPDATE_COMPANYANNUAL, parms);
            }
        }

        #endregion

        #region HELPER METHODS

        private CompanyAnnualData ConvertReaderToCompanyAnnualObject(MySqlDataReader rdr)
        {
            CompanyAnnualData cmp = new CompanyAnnualData();
            cmp.Symbol = MySqlHelper.ConvertReaderToString(rdr, "SYMBOL");
            cmp.Year = MySqlHelper.ConvertReaderToInt(rdr, "YEAR");
            cmp.CostOfDebt = MySqlHelper.ConvertReaderToDecimal(rdr, "COST_OF_DEBT");
            cmp.LeveredBeta = MySqlHelper.ConvertReaderToDecimal(rdr, "LEVERED_BETA");
            cmp.SharesOutstanding = MySqlHelper.ConvertReaderToDecimal(rdr, "SHARES_OUTSTANDING");
            cmp.DividendYield = MySqlHelper.ConvertReaderToDecimal(rdr, "DIVIDEND_YIELD");

            return cmp;
        }

        private static MySqlParameter[] GetCompanyAnnualParameters()
        {
            MySqlParameter[] parms;
            parms = new MySqlParameter[] {
											new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
											new MySqlParameter("@YEAR", MySqlDbType.Int32),
											new MySqlParameter("@COST_OF_DEBT", MySqlDbType.Decimal),
                                            new MySqlParameter("@LEVERED_BETA", MySqlDbType.Decimal),
											new MySqlParameter("@SHARES_OUTSTANDING", MySqlDbType.Decimal),
                                            new MySqlParameter("@DIVIDEND_YIELD", MySqlDbType.Decimal)
											};

            return parms;
        }

        private static void SetCompanyAnnualParameters(CompanyAnnualData comp, MySqlParameter[] parms)
        {
            parms[0].Value = comp.Symbol;
            parms[1].Value = comp.Year;
            parms[2].Value = comp.CostOfDebt;
            parms[3].Value = comp.LeveredBeta;
            parms[4].Value = comp.SharesOutstanding;
            parms[5].Value = comp.DividendYield;
        }

        #endregion
    }
}
