using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;

namespace StockValuationLibrary._2.DAL.DA
{
    public class CompanyDA : ICompanyDA
    {
        #region DATA MEMBERS

        private const string SQL_SELECT_COMPANY = "SELECT * FROM company WHERE SYMBOL = @SYMBOL";
        private const string SQL_SELECT_COMPANIES = "SELECT * FROM company";
        private const string SQL_SELECT_COUNT_COMPANY = "SELECT COUNT(*) FROM company WHERE SYMBOL = @SYMBOL";

        private const string SQL_INSERT_COMPANY =
            @"INSERT INTO company
            (
            SYMBOL,
            COMPANY_NAME,
            INDUSTRY,
            SECTOR)
            VALUES
            (
            @SYMBOL,
            @COMPANY_NAME,
            @INDUSTRY,
            @SECTOR)";

        private const string SQL_UPDATE_COMPANY =
            @"UPDATE company SET
            COMPANY_NAME = @COMPANY_NAME,
            INDUSTRY = @INDUSTRY,
            SECTOR = @SECTOR
            WHERE SYMBOL = @SYMBOL";

        #endregion

        #region CONSTRUCTORS

        private static CompanyDA instance;

        public CompanyDA() { }

        public static CompanyDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyDA();
                }
                return instance;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public Company GetCompany(string tickerSymbol)
        {
            Company cmp = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@SYMBOL", MySqlDbType.VarChar)
            };

            parms[0].Value = tickerSymbol;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_COMPANY, parms))
            {
                if (rdr.Read())
                {
                    cmp = ConvertReaderToCompanyObject(rdr);
                }
            }

            return cmp;
        }

        public CompanyCollection GetCompanies()
        {
            CompanyCollection companies = null;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_COMPANIES))
            {
                if (rdr.Read())
                {
                    //If there is one result
                    companies = new CompanyCollection();
                    //Scroll through the results
                    do
                    {
                        companies.Add(ConvertReaderToCompanyObject(rdr));
                    }
                    while (rdr.Read());
                }
            }

            return companies;
        }

        public int CountCompany(string tickerSymbol)
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
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_SELECT_COUNT_COMPANY, parms));
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion

        #region UPDATE METHODS

        public void InsertCompany(Company company)
        {
            MySqlParameter[] parms = GetCompanyParameters();
            SetCompanyParameters(company, parms);

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_INSERT_COMPANY, parms);
            }

        }

        public void UpdateCompany(Company company)
        {
            MySqlParameter[] parms = GetCompanyParameters();
            SetCompanyParameters(company, parms);

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_UPDATE_COMPANY, parms);
            }
        }

        #endregion

        #region HELPER METHODS

        private Company ConvertReaderToCompanyObject(MySqlDataReader rdr)
        {
            Company cmp = new Company();
            cmp.Symbol = MySqlHelper.ConvertReaderToString(rdr, "SYMBOL");
            cmp.CompanyName = MySqlHelper.ConvertReaderToString(rdr, "COMPANY_NAME");
            cmp.Industry = MySqlHelper.ConvertReaderToString(rdr, "INDUSTRY");
            cmp.Sector = MySqlHelper.ConvertReaderToString(rdr, "SECTOR");

            return cmp;
        }

        private static MySqlParameter[] GetCompanyParameters()
        {
            MySqlParameter[] parms;
            parms = new MySqlParameter[] {
                                            new MySqlParameter("@SYMBOL", MySqlDbType.VarChar),
                                            new MySqlParameter("@COMPANY_NAME", MySqlDbType.VarChar),
                                            new MySqlParameter("@INDUSTRY", MySqlDbType.VarChar),
                                            new MySqlParameter("@SECTOR", MySqlDbType.VarChar)
                							};

            return parms;
        }

        private static void SetCompanyParameters(Company company, MySqlParameter[] parms)
        {
            parms[0].Value = company.Symbol;
            parms[1].Value = company.CompanyName;
            parms[2].Value = company.Industry;
            parms[3].Value = company.Sector;
        }

        #endregion
    }
}
