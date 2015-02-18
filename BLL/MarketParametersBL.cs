using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;
using System.Xml;
using JeffFerguson.Gepsio;


namespace StockValuationLibrary._2.BLL
{
    public class MarketParametersBL
    {
        private IMarketParametersDA _dao;

        #region CONSTRUCTORS

        private static MarketParametersBL instance;

        public static MarketParametersBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MarketParametersBL();
                }
                return instance;
            }
        }

        private MarketParametersBL(IMarketParametersDA dao)
        {
            _dao = dao;
        }

        private MarketParametersBL() : this(new MarketParametersDA()) { }

        #endregion

        #region PUBLIC METHODS

        public MarketParameters GetMarketParameters()
        {
            MarketParameters svParams = _dao.GetMarketParameters();
            return svParams;
        }

        #endregion
    }
}
