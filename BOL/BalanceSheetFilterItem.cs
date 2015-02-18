using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BOL
{
    public class BalanceSheetFilterItem : IFilterItem
    {
        private BalanceSheet _bal;
        private List<Item> _currentAssetsItems;
        private List<Item> _currentLiabilitiesItems;
        private List<Item> _debtItems;
        private List<Item> _shareholderEquityItems;
        private List<Item> _ppeItems;
        private List<Item> _cashItems;
        private List<Item> _totalAssetsItems;
        private XbrlNodeCollection _debtNodes;

        public BalanceSheet Bal
        {
            get { return _bal; }
            set { _bal = value; }
        }

        public List<Item> CurrentAssetsItems
        {
            get { return _currentAssetsItems; }
            set { _currentAssetsItems = value; }
        }  

        public List<Item> CurrentLiabilitiesItems
        {
            get { return _currentLiabilitiesItems; }
            set { _currentLiabilitiesItems = value; }
        }

        public List<Item> DebtItems
        {
            get { return _debtItems; }
            set { _debtItems = value; }
        }
        
        public List<Item> ShareholderEquityItems
        {
            get { return _shareholderEquityItems; }
            set { _shareholderEquityItems = value; }
        }

        public List<Item> PpeItems
        {
            get { return _ppeItems; }
            set { _ppeItems = value; }
        }

        public List<Item> CashItems
        {
            get { return _cashItems; }
            set { _cashItems = value; }
        }

        public List<Item> TotalAssetsItems
        {
            get { return _totalAssetsItems; }
            set { _totalAssetsItems = value; }
        }
        
        public XbrlNodeCollection DebtNodes
        {
            get { return _debtNodes; }
            set { _debtNodes = value; }
        }

        public BalanceSheetFilterItem()
        {
            Initialize();
        }

        public BalanceSheetFilterItem(string symbol, int year)
        {
            this._bal = new BalanceSheet();
            this._bal.Symbol = symbol;
            this._bal.Year = year;

            this._cashItems = new List<Item>();
            this._currentAssetsItems = new List<Item>();
            this._currentLiabilitiesItems = new List<Item>();
            this._debtItems = new List<Item>();
            this._debtNodes = new XbrlNodeCollection();
            this._ppeItems = new List<Item>();
            this._shareholderEquityItems = new List<Item>();
            this._totalAssetsItems = new List<Item>();
            
        }

        #region HELPER METHODS

        private void Initialize()
        {
            this._bal = new BalanceSheet();
            this._cashItems = new List<Item>();
            this._currentAssetsItems = new List<Item>();
            this._currentLiabilitiesItems = new List<Item>();
            this._debtItems = new List<Item>();
            this._debtNodes = new XbrlNodeCollection();
            this._ppeItems = new List<Item>();
            this._shareholderEquityItems = new List<Item>();
            this._totalAssetsItems = new List<Item>();
        }

        #endregion

    }
}
