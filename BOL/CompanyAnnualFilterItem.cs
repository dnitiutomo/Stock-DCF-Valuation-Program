using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BOL
{
    public class CompanyAnnualFilterItem : IFilterItem
    {
        private List<Item> _costOfDebt;

        public List<Item> CostOfDebt
        {
            get { return _costOfDebt; }
            set { _costOfDebt = value; }
        }

        private List<Item> _leveredBeta;

        public List<Item> LeveredBeta
        {
            get { return _leveredBeta; }
            set { _leveredBeta = value; }
        }

        private List<Item> _sharesOutstanding;

        public List<Item> SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set { _sharesOutstanding = value; }
        }

        public CompanyAnnualFilterItem()
            :base()
        {
            Initialize();
        }

        public void Initialize()
        {
            this._costOfDebt = new List<Item>();
            this._leveredBeta = new List<Item>();
            this._sharesOutstanding = new List<Item>();
        }
    }
}
