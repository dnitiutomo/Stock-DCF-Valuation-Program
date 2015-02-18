using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace StockValuationLibrary._2.DAL.DA
{
    public class ColumnMappingsDA : AbstractQueryBuilder
    {
        private static ColumnMappingsDA instance;
        private Dictionary<string, string> columnMappings;
        List<string> columnNames = new List<string>();

        private const string SQL_GET_FIELD_NAMES = "SELECT FIELD_NAME, DESCENDING FROM quandl.column_mappings";
        private const string SQL_GET_ALLOW_ZERO = "SELECT FIELD_NAME, ALLOW_ZERO_VALUE FROM quandl.column_mappings"; 
        private const string SQL_GET_WEIGHTS = "SELECT FIELD_NAME, WEIGHT FROM quandl.column_mappings"; 

        #region CONSTRUCTORS

        public ColumnMappingsDA()
            : base()
        {
            _TABLE_NAME = "quandl.column_mappings";
            _KEY_COLS = new string[] { "QUANDL_COLUMN_NAME" };
            _NON_KEY_COLS = new string[] { "FIELD_NAME" };
            columnMappings = GetColumnMappingsFromDB();
        }

        #endregion

        public Dictionary<string, string> GetColumnMappings()
        {
            return this.columnMappings;
        }

        public static ColumnMappingsDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ColumnMappingsDA();
                }
                return instance;
            }
        }

        private Dictionary<string, string> GetColumnMappingsFromDB()
        {
            Dictionary<string, string> mappings = new Dictionary<string,string>();

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SelectAllSQLStatement()))
            {
                if (rdr.Read())
                {
                    do
                    {
                        string quandlColumnName = MySqlHelper.ConvertReaderToString(rdr, "QUANDL_COLUMN_NAME");
                        string fieldName = MySqlHelper.ConvertReaderToString(rdr, "FIELD_NAME");
                        mappings.Add(quandlColumnName, fieldName);
                        columnNames.Add(quandlColumnName);
                    }
                    while (rdr.Read());
                }
            }

            return mappings;
        }

        public Dictionary<string, bool> GetFieldOrder()
        {
            Dictionary<string, bool> fieldOrders = new Dictionary<string, bool>();

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_GET_FIELD_NAMES))
            {
                if (rdr.Read())
                {
                    do
                    {
                        string fieldName = MySqlHelper.ConvertReaderToString(rdr, "FIELD_NAME");
                        int bit = MySqlHelper.ConvertReaderToInt(rdr, "DESCENDING");

                        if (bit == 1)
                        {
                            fieldOrders.Add(fieldName, true);
                        }
                        else
                        {
                            fieldOrders.Add(fieldName, false);
                        }
                    }
                    while (rdr.Read());
                }
            }

            return fieldOrders;
        }

        public Dictionary<string, bool> GetAllowZeroDictionary()
        {
            Dictionary<string, bool> allowZeroes = new Dictionary<string, bool>();

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_GET_ALLOW_ZERO))
            {
                if (rdr.Read())
                {
                    do
                    {
                        string fieldName = MySqlHelper.ConvertReaderToString(rdr, "FIELD_NAME");
                        int bit = MySqlHelper.ConvertReaderToInt(rdr, "ALLOW_ZERO_VALUE");

                        if (bit == 1)
                        {
                            allowZeroes.Add(fieldName, true);
                        }
                        else
                        {
                            allowZeroes.Add(fieldName, false);
                        }
                    }
                    while (rdr.Read());
                }
            }

            return allowZeroes;
        }

        public Dictionary<string, decimal> GetWeights()
        {
            Dictionary<string, decimal> weights = new Dictionary<string, decimal>();

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_GET_WEIGHTS))
            {
                if (rdr.Read())
                {
                    do
                    {
                        string fieldName = MySqlHelper.ConvertReaderToString(rdr, "FIELD_NAME");
                        decimal weight = MySqlHelper.ConvertReaderToInt(rdr, "WEIGHT");

                        weights.Add(fieldName, weight);
                    }
                    while (rdr.Read());
                }
            }

            return weights;
        }

        public List<string> GetQuandlColumnNames()
        {
            return this.columnNames;

        }
    }
}
