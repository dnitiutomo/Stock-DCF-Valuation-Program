using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class MarketParameter
    {
        #region PRIVATE MEMBERS

        private string _PARAMETER_KEY;
        private string _PARAMETER_VALUE;

        #endregion

        #region PUBLIC PROPERTIES

        public string ParameterKey
        { get { return _PARAMETER_KEY; } set { _PARAMETER_KEY = value; } }

        public string ParameterValue
        { get { return _PARAMETER_VALUE; } set { _PARAMETER_VALUE = value; } }

        #endregion

        #region CONSTRUCTORS

        public MarketParameter() { }

        public MarketParameter(string parameterKey, string parameterValue)
        {
            this._PARAMETER_KEY = parameterKey;
            this._PARAMETER_VALUE = parameterValue;
        }

        #endregion
    }
}
