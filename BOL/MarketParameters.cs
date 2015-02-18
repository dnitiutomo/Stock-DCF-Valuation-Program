using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class MarketParameters
    {
        #region PRIVATE MEMBERS
                                           
        public const string KEY_RISK_FREE_RATE = "RiskFreeRate";     
        public const string KEY_RISK_PREMIUM = "RiskPremium";
        public const string KEY_TAX_RATE = "TaxRate";

        private static Dictionary<string, string> _paramKeyValues = new Dictionary<string, string>
        {
            {"RiskFreeRate", KEY_RISK_FREE_RATE },
            {"RiskPremium" , KEY_RISK_PREMIUM },
            {"TaxRate" , KEY_TAX_RATE }
        };

        private static Dictionary<string, MarketParameter> _parameters;

        #endregion

        #region PUBLIC PROPERTIES

        public Dictionary<string, MarketParameter> StockValuationParameters
        {
            get { return _parameters; }
        }

        public decimal RiskFreeRate
        { 
            get { return Convert.ToDecimal(_parameters[KEY_RISK_FREE_RATE].ParameterValue.Trim()); }
            set { _parameters[KEY_RISK_FREE_RATE].ParameterValue = Convert.ToString(value); }
        }

        public decimal RiskPremium
        {
            get { return Convert.ToDecimal(_parameters[KEY_RISK_PREMIUM].ParameterValue.Trim()); }
            set { _parameters[KEY_RISK_PREMIUM].ParameterValue = Convert.ToString(value); }
        }

        public decimal TaxRate
        {
            get { return Convert.ToDecimal(_parameters[KEY_TAX_RATE].ParameterValue.Trim()); }
            set { _parameters[KEY_TAX_RATE].ParameterValue = Convert.ToString(value); }
        }

        #endregion

        #region CONSTRUCTORS

        private MarketParameters() { }

        public MarketParameters(Dictionary<string, MarketParameter> parameters) 
        {
            _parameters = parameters;
        }

        #endregion
    }
}
