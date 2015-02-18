using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.DA
{
    public class BalanceSheetDA : IBalanceSheetDA
    {
        #region DATA MEMBERS

        private const string SQL_SELECT_BALANCESHEET = "SELECT * FROM balance_sheet WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";
        private const string SQL_SELECT_BALANCESHEETS = "SELECT * FROM balance_sheet WHERE SYMBOL = @SYMBOL AND (YEAR >= @FROMYEAR AND YEAR <= @TOYEAR)";
        private const string SQL_SELECT_COUNT_BALANCESHEET = "SELECT COUNT(*) FROM balance_sheet WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";

        private const string SQL_INSERT_BALANCE_SHEET =
            @"INSERT INTO balance_sheet
            (
            SYMBOL,
            YEAR,
            CURRENT_ASSETS,
            CURRENT_LIABILITIES,
            DEBT,
            SHAREHOLDER_EQUITY,
            PPE,
            CASH,
            TOTAL_ASSETS)
            VALUES
            (
            @SYMBOL,
            @YEAR,
            @CURRENT_ASSETS,
            @CURRENT_LIABILITIES,
            @DEBT,
            @SHAREHOLDER_EQUITY,
            @PPE,
            @CASH,
            @TOTAL_ASSETS)";

        private const string SQL_UPDATE_BALANCE_SHEET =
            @"UPDATE balance_sheet SET
            CURRENT_ASSETS = @CURRENT_ASSETS,
            CURRENT_LIABILITIES = @CURRENT_LIABILITIES,
            DEBT = @DEBT,
            SHAREHOLDER_EQUITY = @SHAREHOLDER_EQUITY,
            PPE = @PPE,
            CASH = @CASH,
            TOTAL_ASSETS = @TOTAL_ASSETS
            WHERE SYMBOL = @SYMBOL AND YEAR = @YEAR";


        #endregion

        #region CONSTRUCTORS

        private static BalanceSheetDA instance;

        public BalanceSheetDA() { }

        public static BalanceSheetDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BalanceSheetDA();
                }
                return instance;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public BalanceSheet GetBalanceSheet(string tickerSymbol, int year)
        {
            BalanceSheet bs = null;
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@YEAR", MySqlDbType.Int16)
            };

            parms[0].Value = tickerSymbol;
            parms[1].Value = year;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_BALANCESHEET, parms))
            {
                if (rdr.Read())
                {
                    bs = ConvertReaderToBalanceSheetObject(rdr);
                }
            }

            return bs;
        }

        public BalanceSheetCollection GetBalanceSheets(string tickerSymbol, int fromYear, int toYear)
        {
            BalanceSheetCollection bs = null;
            BalanceSheet bal = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                new MySqlParameter("@FROMYEAR", MySqlDbType.Int16),
                new MySqlParameter("@TOYEAR", MySqlDbType.Int16)
            };

            parms[0].Value = tickerSymbol;
            parms[1].Value = fromYear;
            parms[2].Value = toYear;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_BALANCESHEETS, parms))
            {
                if (rdr.Read())
                {
                    //If there is one result
                    bs = new BalanceSheetCollection();
                    //Scroll through the results
                    do
                    {
                        bal = ConvertReaderToBalanceSheetObject(rdr);
                        bs.Add(bal);
                    }
                    while (rdr.Read());
                }
            }

            return bs;
        }

        public int CountBalanceSheet(string tickerSymbol, int year)
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
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_SELECT_COUNT_BALANCESHEET, parms));
                }
                catch
                {
                    return 0;
                }
            }

        }

        #endregion

        #region UPDATE METHODS

        public void InsertBalanceSheet(BalanceSheet bs)
        {
            MySqlParameter[] parms = GetBalanceSheetParameters();
            SetBalanceSheetParameters(bs, parms);

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_INSERT_BALANCE_SHEET, parms);
            }
        }

        public void UpdateBalanceSheet(BalanceSheet bs)
        {
            MySqlParameter[] parms = GetBalanceSheetParameters();
            SetBalanceSheetParameters(bs, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_UPDATE_BALANCE_SHEET, parms);
            }
        }


        #endregion

        #region HELPER METHODS

        private BalanceSheet ConvertReaderToBalanceSheetObject(MySqlDataReader rdr)
        {
            BalanceSheet bs = new BalanceSheet();
            bs.Symbol = MySqlHelper.ConvertReaderToString(rdr, "SYMBOL");
            bs.Year = MySqlHelper.ConvertReaderToInt(rdr, "YEAR");
            bs.CurrentAssets = MySqlHelper.ConvertReaderToDecimal(rdr, "CURRENT_ASSETS");
            bs.CurrentLiabilities = MySqlHelper.ConvertReaderToDecimal(rdr, "CURRENT_LIABILITIES");
            bs.Debt = MySqlHelper.ConvertReaderToDecimal(rdr, "DEBT");
            bs.ShareholdersEquity = MySqlHelper.ConvertReaderToDecimal(rdr, "SHAREHOLDER_EQUITY");
            bs.Ppe = MySqlHelper.ConvertReaderToDecimal(rdr, "PPE");
            bs.Cash = MySqlHelper.ConvertReaderToDecimal(rdr, "CASH");
            bs.TotalAssets = MySqlHelper.ConvertReaderToDecimal(rdr, "TOTAL_ASSETS");

            return bs;
        }

        private static MySqlParameter[] GetBalanceSheetParameters()
        {
            MySqlParameter[] parms;
            parms = new MySqlParameter[] {
                                            new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
											new MySqlParameter("@YEAR", MySqlDbType.Int32),
											new MySqlParameter("@CURRENT_ASSETS", MySqlDbType.Decimal),
                                            new MySqlParameter("@CURRENT_LIABILITIES", MySqlDbType.Decimal),
                                            new MySqlParameter("@DEBT", MySqlDbType.Decimal),
                                            new MySqlParameter("@SHAREHOLDER_EQUITY", MySqlDbType.Decimal),
											new MySqlParameter("@PPE", MySqlDbType.Decimal),
											new MySqlParameter("@CASH", MySqlDbType.Decimal),
                                            new MySqlParameter("@TOTAL_ASSETS", MySqlDbType.Decimal)
										    };

            return parms;
        }

        private static void SetBalanceSheetParameters(BalanceSheet bs, MySqlParameter[] parms)
        {
            parms[0].Value = bs.Symbol;
            parms[1].Value = bs.Year;
            parms[2].Value = bs.CurrentAssets;
            parms[3].Value = bs.CurrentLiabilities;
            parms[4].Value = bs.Debt;
            parms[5].Value = bs.ShareholdersEquity;
            parms[6].Value = bs.Ppe;
            parms[7].Value = bs.Cash;
            parms[8].Value = bs.TotalAssets;
        }

        #endregion
    }
}
