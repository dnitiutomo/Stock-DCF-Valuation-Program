using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class FreeCashFlow
    {
        #region PRIVATE MEMBERS

        private string _SYMBOL;
        private int _YEAR;
        private decimal _NOP;
        private decimal _NET_INCOME;
        private decimal _NFA_CHANGE;
        private decimal _WC;
        private decimal _WC_CHANGE;
        private decimal _DEPRECIATION;
        private decimal _INVESTED_CAPITAL;

        #endregion

        #region PUBLIC PROPERTIES

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public int Year
        { get { return _YEAR; } set { _YEAR = value; } }

        public decimal Nop
        { get { return _NOP; } set { _NOP = value; } }

        public decimal NfaChange
        { get { return _NFA_CHANGE; } set { _NFA_CHANGE = value; } }

        public decimal Wc
        { get { return _WC; } set { _WC = value; } }

        public decimal WcChange
        { get { return _WC_CHANGE; } set { _WC_CHANGE = value; } }

        public decimal NetIncome
        { get { return _NET_INCOME; } set { _NET_INCOME = value; } }

        public decimal Depreciation
        { get { return _DEPRECIATION; } set { _DEPRECIATION = value; } }

        public decimal NopCashFlow
        { get { return _NOP + _DEPRECIATION- _NFA_CHANGE - _WC_CHANGE; } }

        public decimal NetIncomeCashFlow
        { get { return (_NET_INCOME *(20/13))  + _DEPRECIATION - _NFA_CHANGE - _WC_CHANGE; } }

        public decimal InvestedCapital
        { get { return _INVESTED_CAPITAL; } }

        public decimal Roic
        { get { return _NOP / _INVESTED_CAPITAL; } }

        #endregion

        #region CONSTRUCTORS

        public FreeCashFlow(
            string symbol, 
            int year,
            decimal nop,
            decimal netIncome,
            decimal nfaChange,
            decimal wc,
            decimal wcChange,
            decimal deprec,
            decimal invCap)
        {
            this._SYMBOL = symbol;
            this._YEAR = year;
            this._NOP = nop;
            this._NFA_CHANGE = nfaChange;
            this._WC = wc;
            this._WC_CHANGE = wcChange;
            this._INVESTED_CAPITAL = invCap;
            this._NET_INCOME = netIncome;
            this._DEPRECIATION = deprec;
        }

        #endregion

    }
}
