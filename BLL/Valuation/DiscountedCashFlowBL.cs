using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BLL;
using StockValuationLibrary._2.BOL;
using MaasOne;
using MaasOne.Base;
using MaasOne.Finance.YahooFinance;

namespace StockValuationLibrary._2.BLL.Valuation
{
    public class DiscountedCashFlowBL
    {
        private static FreeCashFlowBL _fcfMngr;

        public DiscountedCashFlowBL()
        {
            _fcfMngr = new FreeCashFlowBL();
        }

        public void GetDiscountedCashFlows(CompanyValuationStatistics cvs)
        {
            for (int i = -2; i < 3; i++)
            {
                decimal terminalGrowth = (decimal) (i*2) / 100;
                for (int j = 6; j < 20; j++)
                {
                    decimal wacc = (decimal)j / 100;
                    cvs.NopDcfsNoGrowth.Add(GetNopDiscountedCashFlow(cvs, cvs.NoGrowthProjections, wacc, terminalGrowth));
                    cvs.NopDcfsAvgGrowth.Add(GetNopDiscountedCashFlow(cvs, cvs.AvgGrowthProjections, wacc, terminalGrowth));
                    cvs.NopDcfsDecayGrowth.Add(GetNopDiscountedCashFlow(cvs, cvs.DecayGrowthProjections, wacc, terminalGrowth));

                    cvs.NetIncomeDcfsNoGrowth.Add(GetNetIncomeDiscountedCashFlow(cvs, cvs.NoGrowthProjections, wacc, terminalGrowth));
                    cvs.NetIncomeDcfsAvgGrowth.Add(GetNetIncomeDiscountedCashFlow(cvs, cvs.AvgGrowthProjections, wacc, terminalGrowth));
                    cvs.NetIncomeDcfsDecayGrowth.Add(GetNetIncomeDiscountedCashFlow(cvs, cvs.DecayGrowthProjections, wacc, terminalGrowth));
                }
            }    
            
        }

        private DiscountedCashFlow GetNopDiscountedCashFlow(CompanyValuationStatistics cvs, IncomeStatementCollection incProjections,
            decimal wacc, decimal terminalGrowth)
        {
            FreeCashFlowCollection fcfs = _fcfMngr.CalculateFreeCashFlow(cvs.Symbol, cvs.Year, cvs.NoOfYears, incProjections);
            BalanceSheet bs = BalanceSheetBL.Instance.GetBalanceSheet(cvs.Symbol, cvs.Year);
            CompanyAnnualData compData = CompanyAnnualDataBL.Instance.GetCompanyAnnual(cvs.Symbol, cvs.Year);

            DiscountedCashFlow dcf = null;

            if (fcfs != null)
            {
                dcf = new DiscountedCashFlow(cvs.Symbol, cvs.Year, fcfs, wacc, terminalGrowth);
                dcf.EnterpriseValue = 0;
                for (int i = 1; i < cvs.NoOfYears + 1; i++)
                {
                    decimal discountTo = (decimal)(Math.Pow((double)dcf.DiscountFactor, i));
                    dcf.EnterpriseValue += (dcf.FreeCashFlows[i - 1].NopCashFlow * discountTo);
                }

                dcf.EnterpriseValue += (dcf.TerminalValue * (decimal)(Math.Pow((double)dcf.DiscountFactor, dcf.FreeCashFlows.Count)));

                dcf.EquityValue = dcf.EnterpriseValue + bs.Cash - bs.Debt;

                dcf.StockValue = dcf.EquityValue / compData.SharesOutstanding;
            }

            return dcf;
        }

        private DiscountedCashFlow GetNetIncomeDiscountedCashFlow(CompanyValuationStatistics cvs, IncomeStatementCollection incProjections, 
            decimal wacc, decimal terminalGrowth)
        {
            FreeCashFlowCollection fcfs = _fcfMngr.CalculateFreeCashFlow(cvs.Symbol, cvs.Year, cvs.NoOfYears, incProjections);
            DiscountedCashFlow dcf = null;
            BalanceSheet bs = BalanceSheetBL.Instance.GetBalanceSheet(cvs.Symbol, cvs.Year);
            CompanyAnnualData compData = CompanyAnnualDataBL.Instance.GetCompanyAnnual(cvs.Symbol, cvs.Year);

            if (fcfs != null)
            {
                dcf = new DiscountedCashFlow(cvs.Symbol, cvs.Year, fcfs, wacc, terminalGrowth);
                dcf.EnterpriseValue = 0;
                for (int i = 1; i < cvs.NoOfYears + 1; i++)
                {
                    decimal discountTo = (decimal)(Math.Pow((double)dcf.DiscountFactor, i));
                    dcf.EnterpriseValue += (dcf.FreeCashFlows[i - 1].NetIncomeCashFlow * discountTo);
                }

                dcf.EnterpriseValue += (dcf.TerminalValue * (decimal)(Math.Pow((double)dcf.DiscountFactor, dcf.FreeCashFlows.Count)));

                dcf.EquityValue = dcf.EnterpriseValue + bs.Cash - bs.Debt;

                dcf.StockValue = dcf.EquityValue / compData.SharesOutstanding;
            }

            return dcf;
        }

    }
}
