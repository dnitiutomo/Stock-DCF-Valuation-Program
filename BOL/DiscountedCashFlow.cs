using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class DiscountedCashFlow
    {
        #region PRIVATE MEMBERS

        private string _SYMBOL;
        private int _YEAR;
        private FreeCashFlowCollection _freeCashFlows;
        private decimal _WACC;
        private decimal _enterpriseValue;
        private decimal _equityValue;
        private decimal _stockValue;
        private decimal _terminalGrowth;

        #endregion

        #region PUBLIC PROPERTIES

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public int Year
        { get { return _YEAR; } set { _YEAR = value; } }

        public FreeCashFlowCollection FreeCashFlows
        { get { return _freeCashFlows; } set { _freeCashFlows = value; } }

        public decimal Wacc
        { get { return _WACC; } set { _WACC = value; } }

        public decimal TerminalGrowth
        { get { return _terminalGrowth; } set { _terminalGrowth = value; } }

        public decimal StockValue
        { get { return _stockValue; } set { _stockValue = value; } }

        public decimal EnterpriseValue
        { get { return _enterpriseValue; } set { _enterpriseValue = value; } }

        public decimal EquityValue
        { get { return _equityValue; } set { _equityValue = value; } }

        public decimal DiscountFactor
        { get { return 1 / (1 + _WACC); } }

        #endregion

        #region CONSTRUCTORS

        public DiscountedCashFlow(string symbol, int year, FreeCashFlowCollection ffcf, decimal wacc, decimal growth)
        {
            this._SYMBOL = symbol;
            this._YEAR = year;
            this._freeCashFlows = ffcf;
            this._WACC = wacc;
            this._terminalGrowth = growth;
        }

        #endregion

        public decimal TerminalValue
        {
            get
            {
                return (_freeCashFlows[_freeCashFlows.Count - 1].Nop * (1 - (_terminalGrowth / _freeCashFlows[_freeCashFlows.Count - 1].Roic)) / (_WACC - _terminalGrowth));
                //return _freeCashFlows[_freeCashFlows.Count - 1].Nop / (_WACC - _terminalGrowth);
            }
        }
    }
}
