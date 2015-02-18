using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;

namespace StockValuationLibrary._2.DAL.DA
{
    public class MarketParametersDA : IMarketParametersDA
    {
        #region DATA MEMBERS

        private const string SQL_SELECT_SVPARAMETERS = "SELECT * FROM market_parameters";

        #endregion

        #region CONSTRUCTORS

        private static MarketParametersDA instance;

        public MarketParametersDA() { }

        public static MarketParametersDA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MarketParametersDA();
                }
                return instance;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public MarketParameters GetMarketParameters()
        {
            MarketParameters svparams = null;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_SVPARAMETERS))
            {
                Dictionary<string, MarketParameter> svPrms = new Dictionary<string, MarketParameter>();
                if (rdr.Read())
                {
                    do
                    {
                        string parameterKey = MySqlHelper.ConvertReaderToString(rdr, "PARAMETER_KEY");
                        string parameterValue = MySqlHelper.ConvertReaderToString(rdr, "PARAMETER_VALUE");
                        MarketParameter svpara = new MarketParameter(parameterKey, parameterValue);
                        svPrms.Add(parameterKey, svpara);
                    }
                    while (rdr.Read());
                }
                svparams = new MarketParameters(svPrms);
            }

            return svparams;
        }

        #endregion

        #region HELPER METHODS

        #endregion
    }
}
