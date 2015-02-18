using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlIncomeStatementFilter : AbstractXbrlFilter
    {
        public IncomeStatement Populate(XbrlNodeBL nodeMngr, XbrlDocument xbrlDoc, string symbol, int year, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {    
            IncomeStatement inc = new IncomeStatement();
            inc.Symbol = symbol;
            inc.Year = year;

            IncomeStatementFilterItem item = new IncomeStatementFilterItem();

            foreach (XbrlFragment frag in xbrlDoc.XbrlFragments)
            {
                foreach (Item xbrlItem in frag.Facts)
                {
                    if (nodeMngr.IsAnnualItem(xbrlItem) && xbrlItem.Type.Name.Equals("monetaryItemType"))
                    {
                        if (xbrlItem.ContextRef.PeriodEndDate.Year == inc.Year)
                        {
                            item = CheckItem(xbrlItem, item, xbrlTaxonomyTree);
                        }
                    } 
                }
            }
            return PopulateFinancialStatement(item, inc);
        }

        #region HELPER METHODS

        internal IncomeStatementFilterItem CheckItem(Item xbrlItem, IncomeStatementFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            CheckRevenue(xbrlItem, item, xbrlTaxonomyTree);
            CheckCogs(xbrlItem, item, xbrlTaxonomyTree);
            CheckOperatingExpense(xbrlItem, item, xbrlTaxonomyTree);
            CheckDepreciation(xbrlItem, item);
            CheckNetIncome(xbrlItem, item);

            return item;
        }

        internal IncomeStatement PopulateFinancialStatement(IncomeStatementFilterItem item, IncomeStatement inc)
        {
            inc.Revenue = GetMaxValue(item.RevenueItems);
            inc.Cogs = GetMaxValue(item.CogsItems);
            inc.OperatingExpenses = GetMaxValue(item.OperatingItems);
            inc.Depreciation = GetMaxValue(item.DepreciationItems);
            inc.NetIncome = GetMaxValue(item.NetIncomeItems);

            return inc;
        }

        private void CheckRevenue(Item xbrlItem, IncomeStatementFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            if (xbrlItem.Name.Equals("Revenues"))
            {
                item.RevenueItems.Add(xbrlItem);
            }
            else
            {
                foreach (string child in xbrlTaxonomyTree["Revenues"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.RevenueItems.Add(xbrlItem);
                    }
                }
                foreach (string child in xbrlTaxonomyTree["GrossProfit"])
                {
                    if (child.Equals(xbrlItem.Name) && xbrlItem.Name.Contains("Sales"))
                    {
                        item.RevenueItems.Add(xbrlItem);
                    }
                }
                foreach (string child in xbrlTaxonomyTree["OperatingIncomeLoss"])
                {
                    if (child.Equals(xbrlItem.Name) && xbrlItem.Name.Contains("Sales"))
                    {
                        item.RevenueItems.Add(xbrlItem);
                    }
                }
            }
        }

        private void CheckCogs(Item xbrlItem, IncomeStatementFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            if (xbrlItem.Name.Contains("Cost"))
            {
                foreach (string child in xbrlTaxonomyTree["OperatingIncomeLoss"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.CogsItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["GrossProfit"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.CogsItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["EarningsBeforeInterestAndIncomeTaxes"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.CogsItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["IncomeLossFromContinuingOperationsBeforeIncomeTaxesMinorityInterestAndIncomeLossFromEquityMethodInvestments"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.CogsItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["CostOfRevenue"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.CogsItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["CostOfGoodsAndServicesSold"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.CogsItems.Add(xbrlItem);
                    }
                }
            }
        }

        private void CheckOperatingExpense(Item xbrlItem, IncomeStatementFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            if (xbrlItem.Name.Contains("Operating") && xbrlItem.Name.Contains("Expense"))
            {
                item.OperatingItems.Add(xbrlItem);
            }
            else
            {
                foreach (string child in xbrlTaxonomyTree["OperatingIncomeLoss"])
                {
                    if (xbrlItem.Name.Contains("Selling") && child.Equals(xbrlItem.Name))
                    {
                        item.OperatingItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["IncomeLossFromContinuingOperationsBeforeIncomeTaxesMinorityInterestAndIncomeLossFromEquityMethodInvestments"])
                {
                    if (xbrlItem.Name.Contains("Selling") && child.Equals(xbrlItem.Name))
                    {
                        item.OperatingItems.Add(xbrlItem);
                    }
                }

                foreach (string child in xbrlTaxonomyTree["IncomeLossFromContinuingOperationsBeforeInterestExpenseInterestIncomeIncomeTaxesExtraordinaryItemsNoncontrollingInterestsNet"])
                {
                    if (xbrlItem.Name.Contains("Selling") && child.Equals(xbrlItem.Name))
                    {
                        item.OperatingItems.Add(xbrlItem);
                    }
                }
            }
        }

        private void CheckDepreciation(Item xbrlItem, IncomeStatementFilterItem item)
        {
            if (xbrlItem.Name.Contains("Depreciation"))
            {
                item.DepreciationItems.Add(xbrlItem);
            }
        }

        private void CheckNetIncome(Item xbrlItem, IncomeStatementFilterItem item)
        {
            if (xbrlItem.Name.Contains("NetIncomeLoss"))
            {
                item.NetIncomeItems.Add(xbrlItem);
            }
            else if (xbrlItem.Name.Contains("ProfitLoss"))
            {
                item.NetIncomeItems.Add(xbrlItem);
            }
        }

        #endregion
    }
}
