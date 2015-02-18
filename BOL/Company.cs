using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class Company
    {
        #region PUBLIC MEMBERS

        private string _SYMBOL;
        private string _COMPANY_NAME;
        private string _INDUSTRY;
        private string _SECTOR;

        #endregion

        #region PUBLIC PROPERTIES

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public string CompanyName
        { get { return _COMPANY_NAME; } set { _COMPANY_NAME = value; } }

        public string Industry
        { get { return _INDUSTRY; } set { _INDUSTRY = value; } }

        public string Sector
        { get { return _SECTOR; } set { _SECTOR = value; } }

        #endregion

        #region CONSTRUCTORS

        public Company(string symbol, string name, string industry, string sector)
        {
            this._SYMBOL = symbol;
            this._COMPANY_NAME = name;
            this._INDUSTRY = industry;
            this._SECTOR = sector;
        }

        public Company()
        {
            Initialize();
        }

        #endregion

        #region HELPER METHODS

        private void Initialize()
        {
            this._SYMBOL = string.Empty;
            this._COMPANY_NAME = string.Empty;
            this._INDUSTRY = string.Empty;
            this._SECTOR = string.Empty;
        }

        #endregion
    }
}
