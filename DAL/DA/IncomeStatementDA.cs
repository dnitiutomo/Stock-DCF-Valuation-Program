using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using MySql.Data.MySqlClient;

namespace StockValuationLibrary._2.DAL.DA
{
    public class IncomeStatementDA : IIncomeStatementDA
    {
        #region DATA MEMBERS

        private const string SQL_SELECT_INC = "SELECT * FROM income_statement WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";
        private const string SQL_SELECT_INCS = "SELECT * FROM income_statement WHERE SYMBOL = @SYMBOL AND (YEAR >= @FROMYEAR AND YEAR <= @TOYEAR)";
        private const string SQL_SELECT_COUNT_INCOMESTATEMENT = "SELECT COUNT(*) FROM income_statement WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";
        private const string SQL_SELECT_MAX_YEAR = "SELECT MAX(YEAR) FROM income_statement WHERE SYMBOL = @SYMBOL";

        private const string SQL_INSERT_INCOMESTATEMENT =
            @"INSERT INTO income_statement
            (SYMBOL,
            YEAR,
            REVENUE,
            COGS,
            OPERATING_EXPENSES,
            DEPRECIATION,
            NET_INCOME,
            CAPITAL_EXPENDITURES)
            VALUES
            (@SYMBOL,
            @YEAR,
            @REVENUE,
            @COGS,
            @OPERATING_EXPENSES,
            @DEPRECIATION,
            @NET_INCOME,
            @CAPITAL_EXPENDITURES
            )";

        private const string SQL_UPDATE_INCOMESTATEMENT =
            @"UPDATE income_statement SET
            REVENUE = @REVENUE,
            COGS = @COGS,
            OPERATING_EXPENSES = @OPERATING_EXPENSES,
            DEPRECIATION = @DEPRECIATION,
            NET_INCOME = @NET_INCOME,
            CAPITAL_EXPENDITURES = @CAPITAL_EXPENDITURES
            WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";

        #endregion

        #region CONSTRUCTORS

        private static IncomeStatementDA instance;

        public IncomeStatementDA() { }

        public static IncomeStatementDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IncomeStatementDA();
                }
                return instance;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public IncomeStatement GetIncomeStatement(string tickerSymbol, int year)
        {
            IncomeStatement incstm = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@YEAR", MySqlDbType.Int16)
            };


            parms[0].Value = tickerSymbol;
            parms[1].Value = year;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_INC, parms))
            {
                if (rdr.Read())
                {
                    incstm = ConvertReaderToIncomeStatementObject(rdr);
                }
            }

            return incstm;
        }

        public IncomeStatementCollection GetIncomeStatements(string tickerSymbol, int fromYear, int toYear)
        {
            IncomeStatementCollection incs = null;
            IncomeStatement inc = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@FROMYEAR", MySqlDbType.Int16),
                new MySqlParameter("@TOYEAR", MySqlDbType.Int16)
            };

            parms[0].Value = tickerSymbol;
            parms[1].Value = fromYear;
            parms[2].Value = toYear;

            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_INCS, parms))
            {
                if (rdr.Read())
                {
                    //If there is one result
                    incs = new IncomeStatementCollection();
                    //Scroll through the results
                    do
                    {
                        inc = ConvertReaderToIncomeStatementObject(rdr);
                        if (inc.Revenue != 0)
                        {
                            incs.Add(inc);
                        }
                    }
                    while (rdr.Read());
                }
            }

            return incs;
        }

        public int CountIncomeStatement(string tickerSymbol, int year)
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
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_SELECT_COUNT_INCOMESTATEMENT, parms));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int GetLastIncomeStatementYear(string tickerSymbol)
        {
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar)
            };
            parms[0].Value = tickerSymbol;

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                try
                {
                    conn.Open();
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_SELECT_MAX_YEAR, parms));
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion

        #region UPDATE METHODS

        public void InsertIncomeStatement(IncomeStatement inc)
        {
            MySqlParameter[] parms = GetIncomeStatementParameters();
            SetIncomeStatementParameters(inc, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_INSERT_INCOMESTATEMENT, parms);
            }
        }

        public void UpdateIncomeStatement(IncomeStatement inc)
        {
            MySqlParameter[] parms = GetIncomeStatementParameters();
            SetIncomeStatementParameters(inc, parms);

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_UPDATE_INCOMESTATEMENT, parms);
            }
        }

        #endregion


        #region HELPER METHODS

        private IncomeStatement ConvertReaderToIncomeStatementObject(MySqlDataReader rdr)
        {
            IncomeStatement incstm = new IncomeStatement();
            incstm.Symbol = MySqlHelper.ConvertReaderToString(rdr, "SYMBOL");
            incstm.Year = MySqlHelper.ConvertReaderToInt(rdr, "YEAR");
            incstm.Revenue = MySqlHelper.ConvertReaderToDecimal(rdr, "REVENUE");
            incstm.Cogs = MySqlHelper.ConvertReaderToDecimal(rdr, "COGS");
            incstm.OperatingExpenses = MySqlHelper.ConvertReaderToDecimal(rdr, "OPERATING_EXPENSES");
            incstm.Depreciation = MySqlHelper.ConvertReaderToDecimal(rdr, "DEPRECIATION");
            incstm.NetIncome = MySqlHelper.ConvertReaderToDecimal(rdr, "NET_INCOME");
            incstm.CapitalExpenditures = MySqlHelper.ConvertReaderToDecimal(rdr, "CAPITAL_EXPENDITURES");

            return incstm;
        }

        private static MySqlParameter[] GetIncomeStatementParameters()
        {
            MySqlParameter[] parms;
            parms = new MySqlParameter[] {
											new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
											new MySqlParameter("@YEAR", MySqlDbType.Int32),
											new MySqlParameter("@REVENUE", MySqlDbType.Decimal),
											new MySqlParameter("@COGS", MySqlDbType.Decimal),
											new MySqlParameter("@OPERATING_EXPENSES", MySqlDbType.Decimal),
											new MySqlParameter("@DEPRECIATION", MySqlDbType.Decimal),
                                            new MySqlParameter("@NET_INCOME", MySqlDbType.Decimal),
                                            new MySqlParameter("@CAPITAL_EXPENDITURES", MySqlDbType.Decimal),
											};

            return parms;
        }

        private static void SetIncomeStatementParameters(IncomeStatement inc, MySqlParameter[] parms)
        {
            parms[0].Value = inc.Symbol;
            parms[1].Value = inc.Year;
            parms[2].Value = inc.Revenue;
            parms[3].Value = inc.Cogs;
            parms[4].Value = inc.OperatingExpenses;
            parms[5].Value = inc.Depreciation;
            parms[6].Value = inc.NetIncome;
            parms[7].Value = inc.CapitalExpenditures;
        }

        #endregion
    }
}
