using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlCompanyAnnualFilter : AbstractXbrlFilter
    {
        #region FILTER METHODS

        public CompanyAnnualData Populate(XbrlDocument xbrlDoc, string symbol, int year)
        {
            CompanyAnnualData ann = new CompanyAnnualData();
            ann.Symbol = symbol;
            ann.Year = year;

            CompanyAnnualFilterItem item = new CompanyAnnualFilterItem();

            foreach (XbrlFragment frag in xbrlDoc.XbrlFragments)
            {
                foreach (Item xbrlItem in frag.Facts)
                {
                    if (xbrlItem.ContextRef.InstantDate.Year == ann.Year)
                    {
                        if (!xbrlItem.Type.Name.Equals("monetaryItemType"))
                        {
                            item = CheckItem(xbrlItem, item);
                        }      
                    }
                }
            }
            ann = PopulateFinancialStatement(item, ann);

            if (ann.SharesOutstanding < 0)
            {
                ann.SharesOutstanding = ann.SharesOutstanding * -1;
            }
            if (ann.SharesOutstanding < 1000)
            {
                ann.SharesOutstanding = ann.SharesOutstanding * 1000000;
            }
            else if (ann.SharesOutstanding < 1000000)
            {
                ann.SharesOutstanding = ann.SharesOutstanding * 1000;
            }

            return ann;
        }

        internal CompanyAnnualFilterItem CheckItem(Item xbrlItem, CompanyAnnualFilterItem item)
        {
            CheckCostOfDebt(xbrlItem, ref item);
            CheckSharesOutstanding(xbrlItem, ref item);

            return item;
        }

        internal CompanyAnnualData PopulateFinancialStatement(CompanyAnnualFilterItem item, CompanyAnnualData ann)
        {
            ann.CostOfDebt = GetAvgValue(item.CostOfDebt);
            ann.SharesOutstanding = GetMaxValue(item.SharesOutstanding);

            return ann;
        }

        private void CheckCostOfDebt(Item xbrlItem, ref CompanyAnnualFilterItem item)
        {
            if (xbrlItem.Type.Name.Equals("percentItemType") && xbrlItem.Name.Contains("InterestRate"))
            {
                item.CostOfDebt.Add(xbrlItem);
            }
        }

        private void CheckSharesOutstanding(Item xbrlItem, ref CompanyAnnualFilterItem item)
        {
            if (xbrlItem.Type.Name.Equals("sharesItemType"))
            {
                if (xbrlItem.Name.Contains("CommonStockSharesOutstanding") || xbrlItem.Name.Contains("CommonStockSharesIssued"))
                {
                    item.SharesOutstanding.Add(xbrlItem);
                } 
            }
        }

        #endregion

    }
}
