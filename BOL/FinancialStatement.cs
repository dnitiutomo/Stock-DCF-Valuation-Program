using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public abstract class FinancialStatement
    {
        #region PRIVATE MEMBERS

        protected string _SYMBOL;
        protected int _YEAR;

        #endregion

        #region PUBLIC PROPERTIES

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public int Year
        { get { return _YEAR; } set { _YEAR = value; } }

        #endregion

        #region CONSTRUCTORS

        public FinancialStatement()
        {
            Initialize();
        }

        #endregion

        #region HELPER METHODS

        private void Initialize()
        {
            this._SYMBOL = string.Empty;
            this._YEAR = 0;
        }

        #endregion
    }
}
