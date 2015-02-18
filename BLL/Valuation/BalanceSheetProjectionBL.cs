using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using StockValuationLibrary._2.BLL;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL.Valuation
{
    public class BalanceSheetProjectionBL
    {
        public BalanceSheetCollection ProjectBalanceSheets(string symbol, int fromYear, int yrsToProject, IncomeStatementCollection incs)
        {
            BalanceSheetCollection balProjections =  BalanceSheetBL.Instance.GetBalanceSheets(symbol, fromYear - 2, fromYear);

            if (balProjections == null || balProjections.Count < 1)
            {
                throw new Exception("Not enough Balance Sheet Data");
            }

            for (int i = fromYear + 1; i < fromYear + yrsToProject + 1; i++)
            {
                IncomeStatement thisInc = incs.Find(i);
                if (thisInc != null)
                {
                    balProjections.Add(new BalanceSheet(
                        incs[0].Symbol,
                        i,
                        thisInc.Revenue * GetAvgPercentOfSales("CurrentAssets", balProjections, incs),
                        thisInc.Revenue * GetAvgPercentOfSales("CurrentLiabilities", balProjections, incs),
                        thisInc.Revenue * GetAvgPercentOfSales("Ppe", balProjections, incs),
                        thisInc.Revenue * GetAvgPercentOfSales("Cash", balProjections, incs),
                        thisInc.Revenue * GetAvgPercentOfSales("Debt", balProjections, incs),
                        thisInc.Revenue * GetAvgPercentOfSales("ShareholdersEquity", balProjections, incs),
                        thisInc.Revenue * GetAvgPercentOfSales("TotalAssets", balProjections, incs)
                        ));
                }
            }

            return balProjections;
        }


        internal decimal GetAvgPercentOfSales(string itemName, BalanceSheetCollection balProjections, IncomeStatementCollection incs)
        {
            decimal[] salesMargin = new decimal[balProjections.Count];

            for (int i = 0; i < salesMargin.Length; i++)
            {
                PropertyInfo propInfo = typeof(BalanceSheet).GetProperty(itemName);
                decimal value = (decimal)propInfo.GetValue(balProjections[i], null);

                salesMargin[i] = value / incs[i].Revenue;
            }

            decimal sum = 0;
            foreach (decimal perc in salesMargin)
            {
                sum += perc;
            }
            return sum / salesMargin.Length;
        }
    }
}
