using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlBalanceSheetFilter : AbstractXbrlFilter
    {
        public BalanceSheet Populate(XbrlDocument xbrlDoc, string symbol, int year, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            BalanceSheet bal = new BalanceSheet();
            bal.Symbol = symbol;
            bal.Year = year;

            BalanceSheetFilterItem item = new BalanceSheetFilterItem();

            foreach (XbrlFragment frag in xbrlDoc.XbrlFragments)
            {
                foreach (Item xbrlItem in frag.Facts)
                {
                    if (xbrlItem.ContextRef.InstantPeriod && xbrlItem.Type.Name.Equals("monetaryItemType"))
                    {
                        if (xbrlItem.ContextRef.InstantDate.Year == bal.Year)
                        {
                            item = CheckItem(xbrlItem, item, xbrlTaxonomyTree);
                        }
                    }
                }
            }
            return PopulateFinancialStatement(item,bal);
        }

        #region HELPER METHODS

        internal BalanceSheetFilterItem CheckItem(Item xbrlItem, BalanceSheetFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            CheckCurrentAssets(xbrlItem, ref item);
            CheckCurrentLiabilities(xbrlItem, ref item);
            CheckCash(xbrlItem,ref item, xbrlTaxonomyTree);
            CheckDebt(xbrlItem, ref item);
            CheckPpe(xbrlItem, ref item, xbrlTaxonomyTree);
            CheckShareholdersEquity(xbrlItem, ref item);
            CheckTotalAssets(xbrlItem,ref item);

            return item;
        }

        internal BalanceSheet PopulateFinancialStatement(BalanceSheetFilterItem item, BalanceSheet bs)
        {
            bs.CurrentAssets = GetMaxValue(item.CurrentAssetsItems);
            bs.CurrentLiabilities = GetMaxValue(item.CurrentLiabilitiesItems);
            bs.Debt = GetMaxValue(item.DebtItems);
            bs.Ppe = GetMaxValue(item.PpeItems);
            bs.ShareholdersEquity = GetMaxValue(item.ShareholderEquityItems);
            bs.Cash = GetMaxValue(item.CashItems);
            bs.TotalAssets = GetMaxValue(item.TotalAssetsItems);

            return bs;
        }

        private void CheckCurrentAssets(Item xbrlItem, ref BalanceSheetFilterItem item)
        {
            if (xbrlItem.Name.Equals("AssetsCurrent"))
            {
                item.CurrentAssetsItems.Add(xbrlItem);
            }
        }

        private void CheckCurrentLiabilities(Item xbrlItem, ref BalanceSheetFilterItem item)
        {
            if (xbrlItem.Name.Equals("LiabilitiesCurrent"))
            {
                item.CurrentLiabilitiesItems.Add(xbrlItem);
            }
        }

        private void CheckDebt(Item xbrlItem, ref BalanceSheetFilterItem item)
        {
            if (xbrlItem.Name.Contains("LongTermDebt"))
            {
                item.DebtItems.Add(xbrlItem);
            }
            else
            {
                foreach (XbrlNode node in item.DebtNodes)
                {
                    if (xbrlItem.Name.Equals(node.nodeId))
                    {
                        item.DebtItems.Add(xbrlItem);
                    }
                }
            }
        }

        private void CheckPpe(Item xbrlItem, ref BalanceSheetFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            if (xbrlItem.Name.Equals("PropertyPlantAndEquipmentNet"))
            {
                item.PpeItems.Add(xbrlItem);
            }
            else
            {
                foreach (string child in xbrlTaxonomyTree["PropertyPlantAndEquipmentNet"])
                {
                    if (child.Equals(xbrlItem.Name))
                    {
                        item.PpeItems.Add(xbrlItem);
                    }
                }
            }
        }

        private void CheckShareholdersEquity(Item xbrlItem, ref BalanceSheetFilterItem item)
        {
            if (xbrlItem.Name.Equals("StockholdersEquity"))
            {
                item.ShareholderEquityItems.Add(xbrlItem);
            }
            else if (xbrlItem.Name.Equals("StockholdersEquityIncludingPortionAttributableToNoncontrollingInterest"))
            {
                item.ShareholderEquityItems.Add(xbrlItem);
            }
        }

        private void CheckCash(Item xbrlItem, ref BalanceSheetFilterItem item, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            if (xbrlItem.Name.Equals("Cash"))
            {
                item.CashItems.Add(xbrlItem);
            }
            else
            {
                foreach (string child in xbrlTaxonomyTree["AssetsCurrent"])
                {
                    if (child.Equals(xbrlItem.Name) && xbrlItem.Name.Contains("Cash"))
                    {
                        item.CashItems.Add(xbrlItem);
                    }
                }
            }
        }

        private void CheckTotalAssets(Item xbrlItem, ref BalanceSheetFilterItem item)
        {
            if (xbrlItem.Name.Equals("Assets"))
            {
                item.TotalAssetsItems.Add(xbrlItem);
            }
        }

        #endregion

    }
}
