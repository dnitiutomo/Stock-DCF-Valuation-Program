using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using StockValuationLibrary._2.BLL;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL.Valuation
{
    public class IncomeStatementProjectionBL
    {
        public IncomeStatementCollection ProjectIncomeStatementsNoRevenueGrowth(string symbol, int fromYear, int yrsToProject)
        {
            IncomeStatementCollection incProjections = IncomeStatementBL.Instance.GetIncomeStatements(symbol, fromYear - 2, fromYear);
            CheckEnoughIncomeStatements(incProjections);

            for (int i = fromYear + 1; i < fromYear + yrsToProject + 1; i++)
            {
                IncomeStatement lastYrInc = incProjections.Find(i - 1);
                if (lastYrInc != null)
                {
                    incProjections.Add(new IncomeStatement(
                        incProjections[0].Symbol,
                        i,
                        lastYrInc.Revenue,
                        lastYrInc.Revenue * GetAvgPercentOfSales("Cogs", incProjections),
                        lastYrInc.Revenue * GetAvgPercentOfSales("OperatingExpenses", incProjections),
                        lastYrInc.Revenue * GetAvgPercentOfSales("Depreciation", incProjections),
                        lastYrInc.Revenue * GetAvgPercentOfSales("NetIncome", incProjections),
                        lastYrInc.Revenue * GetAvgPercentOfSales("CapitalExpenditures", incProjections)
                        ));
                }
            }

            return incProjections;
        }

        public IncomeStatementCollection ProjectIncomeStatementsDecayRevenueGrowth(string symbol, int fromYear, int yrsToProject, decimal endGrowth)
        {
            IncomeStatementCollection incProjections = IncomeStatementBL.Instance.GetIncomeStatements(symbol, fromYear - 2, fromYear);
            CheckEnoughIncomeStatements(incProjections);

            decimal[] yoyGrowth = GetDecayGrowthRates(yrsToProject, endGrowth, incProjections);

            int j = 0;

            for (int i = fromYear + 1; i < fromYear + yrsToProject + 1; i++)
            {
                IncomeStatement lastYrInc = incProjections.Find(i - 1);
                if (lastYrInc != null)
                {
                    incProjections.Add(new IncomeStatement(
                        incProjections[0].Symbol,
                        i,
                        lastYrInc.Revenue * (1 + yoyGrowth[j]),
                        lastYrInc.Revenue * (1 + yoyGrowth[j]) * GetAvgPercentOfSales("Cogs",incProjections),
                        lastYrInc.Revenue * (1 + yoyGrowth[j]) * GetAvgPercentOfSales("OperatingExpenses", incProjections),
                        lastYrInc.Revenue * (1 + yoyGrowth[j]) * GetAvgPercentOfSales("Depreciation", incProjections),
                        lastYrInc.Revenue * (1 + yoyGrowth[j]) * GetAvgPercentOfSales("NetIncome", incProjections),
                        lastYrInc.Revenue * (1 + yoyGrowth[j]) * GetAvgPercentOfSales("CapitalExpenditures", incProjections)
                        ));
                }
                j++;
            }

            return incProjections;
        }

        public IncomeStatementCollection ProjectIncomeStatementsAvgRevenueGrowth(string symbol, int fromYear, int yrsToProject)
        {
            IncomeStatementCollection incProjections = IncomeStatementBL.Instance.GetIncomeStatements(symbol, fromYear - 2, fromYear);
            CheckEnoughIncomeStatements(incProjections);
            decimal avgGrowthRate = GetRevenueAverageGrowthRate(incProjections);
            for (int i = fromYear + 1; i < fromYear + yrsToProject + 1; i++)
            {
                IncomeStatement lastYrInc = incProjections.Find(i-1);
                if (lastYrInc != null)
                {
                    incProjections.Add(new IncomeStatement(
                        incProjections[0].Symbol,
                        i,
                        lastYrInc.Revenue * (1 + avgGrowthRate),
                        lastYrInc.Revenue * (1 + avgGrowthRate) * GetAvgPercentOfSales("Cogs", incProjections),
                        lastYrInc.Revenue * (1 + avgGrowthRate) * GetAvgPercentOfSales("OperatingExpenses", incProjections),
                        lastYrInc.Revenue * (1 + avgGrowthRate) * GetAvgPercentOfSales("Depreciation", incProjections),
                        lastYrInc.Revenue * (1 + avgGrowthRate) * GetAvgPercentOfSales("NetIncome", incProjections),
                        lastYrInc.Revenue * (1 + avgGrowthRate) * GetAvgPercentOfSales("CapitalExpenditures", incProjections)
                        ));
                }
            }

            return incProjections;
        }

        private decimal[] GetDecayGrowthRates(int yearsToProject, decimal endGrowth, IncomeStatementCollection incProjections)
        {
            decimal beginGrowth = 0;

            decimal[] yoyFutureGrowth = new decimal[yearsToProject];

            decimal increment = (beginGrowth - endGrowth) / (yearsToProject);

            yoyFutureGrowth[0] = beginGrowth;

            for (int i = 1; i < yearsToProject; i++)
            {
                yoyFutureGrowth[i] = yoyFutureGrowth[i - 1] - increment;
            }

            return yoyFutureGrowth;
        }

        private decimal GetAvgPercentOfSales(string itemName, IncomeStatementCollection incProjections)
        {
            decimal[] salesMargin = new decimal[incProjections.Count];

            for (int i = 0; i < salesMargin.Length; i++)
            {
                PropertyInfo propInfo = typeof(IncomeStatement).GetProperty(itemName);
                decimal value = (decimal)propInfo.GetValue(incProjections[i], null);

                salesMargin[i] = value / incProjections[i].Revenue;
            }

            decimal sum = 0;
            foreach (decimal perc in salesMargin)
            {
                sum += perc;
            }
            return sum / salesMargin.Length;
        }


        private decimal GetRevenueAverageGrowthRate(IncomeStatementCollection incProjections)
        {
            decimal[] yoyGrowth = new decimal[incProjections.Count - 1];
            for (int i = 0; i < yoyGrowth.Length; i++)
            {
                yoyGrowth[i] = (incProjections[i + 1].Revenue - incProjections[i].Revenue) / incProjections[i].Revenue;
            }

            decimal sum = 0;
            foreach (decimal perc in yoyGrowth)
            {
                sum += perc;
            }

            return sum / yoyGrowth.Length;
        }

        private void CheckEnoughIncomeStatements(IncomeStatementCollection incs)
        {
            if (incs.Count < 2)
            {
                throw new Exception("Not enough Income Statements");
            }
            else if (incs[0].Revenue == 0 || incs[1].Revenue == 0)
            {
                throw new Exception("Income Statement fields, Revenue, null");
            }
        }
    }
}
