using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BOL
{
    public class IncomeStatementFilterItem : IFilterItem
    {
        private List<Item> _revenueItems;

        public List<Item> RevenueItems
        {
            get { return _revenueItems; }
            set { _revenueItems = value; }
        }

        private List<Item> _cogsItems;

        public List<Item> CogsItems
        {
            get { return _cogsItems; }
            set { _cogsItems = value; }
        }

        private List<Item> _operatingItems;

        public List<Item> OperatingItems
        {
            get { return _operatingItems; }
            set { _operatingItems = value; }
        }

        private List<Item> _depreciationItems;

        public List<Item> DepreciationItems
        {
            get { return _depreciationItems; }
            set { _depreciationItems = value; }
        }

        private List<Item> _netIncomeItems;

        public List<Item> NetIncomeItems
        {
            get { return _netIncomeItems; }
            set { _netIncomeItems = value; }
        }

        public IncomeStatementFilterItem()
        {
            Initialize();
        }

        #region HELPER METHODS

        private void Initialize()
        {
            this._cogsItems = new List<Item>();
            this._depreciationItems = new List<Item>();
            this._netIncomeItems = new List<Item>();
            this._operatingItems = new List<Item>();
            this._revenueItems = new List<Item>();
        }

        #endregion 
    }
}
