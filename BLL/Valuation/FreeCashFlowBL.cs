using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BLL;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL.Valuation
{
    public class FreeCashFlowBL
    {
        public FreeCashFlowCollection CalculateFreeCashFlow(string symbol, int year, int yrsToProject ,IncomeStatementCollection incomeStatementProjection)
        {
            FreeCashFlowCollection fcfs = null;

            MarketParameters mktParms = MarketParametersBL.Instance.GetMarketParameters();

            BalanceSheetProjectionBL bsProjectionBL = new BalanceSheetProjectionBL();

            BalanceSheetCollection bsProjections = bsProjectionBL.ProjectBalanceSheets(symbol, year, yrsToProject, incomeStatementProjection);

            if (incomeStatementProjection != null && bsProjections != null)
            {
                fcfs = new FreeCashFlowCollection();
                for (int i = year + 1; i < year + yrsToProject + 1; i++)
                {
                    IncomeStatement inc = incomeStatementProjection.Find(i);
                    BalanceSheet bs = bsProjections.Find(i);
                    BalanceSheet lastYrBs = bsProjections.Find(i - 1);

                    if (inc != null && bs != null)
                    {
                        fcfs.Add(new FreeCashFlow(
                            symbol,
                            i,
                            inc.NOP(mktParms.TaxRate),
                            inc.NetIncome,
                            (bs.Ppe - lastYrBs.Ppe + inc.Depreciation),
                            bs.WorkingCapital,
                            bs.WorkingCapital -lastYrBs.WorkingCapital,
                            inc.Depreciation,
                            bs.InvestedCapital
                            ));
                    }
                }
            }

            return fcfs;
        }
    }
}
