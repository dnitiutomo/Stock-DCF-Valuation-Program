using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using StockValuationLibrary._2.BLL.Valuation;
using System.IO;
using OfficeOpenXml;

namespace StockValuationLibrary._2.BLL
{
    public class CreateExcelDocument
    {
        private static DiscountedCashFlowBL _dcfMngr;
        private static CompanyFinancialStatisticsBL _fsMngr;

        public void CreateAbbreviatedExcelDocument()
        {
            string filename = @"C:\Users\Daniel\Documents\Visual Studio 2012\ExcelFiles\abbreviated_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + ".xlsx";

            FileInfo newFile = new FileInfo(filename);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                string[] columns = AbrreviatedColumns();

                ExcelWorksheet compInfoWS = xlPackage.Workbook.Worksheets.Add("Company Info");

                int i = 1;
                compInfoWS.Row(1).Style.Font.Bold = true;
                foreach (string col in columns)
                {
                    compInfoWS.Cells[1, i].Value = col;
                    i++;
                }

                CompanyCollection comps = CompanyBL.Instance.GetCompanies();

                CompanyValuationStatisticsCollection valuations = new CompanyValuationStatisticsCollection();
                CompanyFinancialStatisticsCollection statistics = new CompanyFinancialStatisticsCollection();
                CompanyValuationStatistics cvs;

                int count = 2;
                int x = 0;
                foreach (Company comp in comps)
                {
                    x++;
                    //if (comp.Symbol.Equals("azo"))
                    //{
                        try
                        {
                            cvs = GetCompanyValuationStatistics(comp);
                            valuations.Add(cvs);
                            CompanyFinancialStatistics stats = CompanyFinancialStatisticsBL.Instance.GetCompanyFinancialStatistics(comp.Symbol, DateTime.Today.AddDays(-1));
                            statistics.Add(stats);
                            BalanceSheet bal = BalanceSheetBL.Instance.GetBalanceSheet(comp.Symbol, 2012);
                            IncomeStatement inc = IncomeStatementBL.Instance.GetIncomeStatement(comp.Symbol, 2012);
                            CompanyAnnualData data = CompanyAnnualDataBL.Instance.GetCompanyAnnual(comp.Symbol, 2012);

                            int f = 1;

                            compInfoWS.Cells[count, f++].Value = comp.Symbol;
                            compInfoWS.Cells[count, f++].Value = comp.CompanyName;
                            compInfoWS.Cells[count, f++].Value = comp.Industry;
                            compInfoWS.Cells[count, f++].Value = comp.Sector;
                            compInfoWS.Cells[count, f++].Value = stats.StockPrice;
                            compInfoWS.Cells[count, f++].Value = stats.BookValuePerShare;
                            compInfoWS.Cells[count, f++].Value = stats.ReturnOnEquity;
                            compInfoWS.Cells[count, f++].Value = stats.ReturnOnAssets;
                            compInfoWS.Cells[count, f++].Value = stats.DebtToEquity;
                            compInfoWS.Cells[count, f++].Value = stats.YearHigh;
                            compInfoWS.Cells[count, f++].Value = stats.YearLow;
                            compInfoWS.Cells[count, f++].Value = stats.Eps;
                            compInfoWS.Cells[count, f++].Value = stats.RevenuePerShare;
                            compInfoWS.Cells[count, f++].Value = stats.AverageVolume;
                            compInfoWS.Cells[count, f++].Value = stats.MarketCap;
                            compInfoWS.Cells[count, f++].Value = stats.TotalDebt;
                            compInfoWS.Cells[count, f++].Value = stats.CurrentSharesShort;
                            compInfoWS.Cells[count, f++].Value = stats.LastMonthSharesShort;
                            compInfoWS.Cells[count, f++].Value = (stats.CurrentSharesShort / data.SharesOutstanding);
                            compInfoWS.Cells[count, f++].Value = (stats.LastMonthSharesShort / data.SharesOutstanding);
                            compInfoWS.Cells[count, f++].Value = stats.FiftyMovingAverage;
                            compInfoWS.Cells[count, f++].Value = stats.TwoHundredMovingAverage;
                            compInfoWS.Cells[count, f++].Value = stats.DividendYieldPercent;
                            compInfoWS.Cells[count, f++].Value = stats.OperatingMargin;
                            compInfoWS.Cells[count, f++].Value = stats.ProfitMargin;
                            compInfoWS.Cells[count, f++].Value = stats.TrailingPE;
                            compInfoWS.Cells[count, f++].Value = stats.ForwardPE;
                            compInfoWS.Cells[count, f++].Value = stats.PegRatio;
                            compInfoWS.Cells[count, f++].Value = stats.EnterpriseValue;
                            compInfoWS.Cells[count, f++].Value = stats.PriceToSales;
                            compInfoWS.Cells[count, f++].Value = stats.PriceToBook;
                            compInfoWS.Cells[count, f++].Value = stats.EvToRevenue;
                            compInfoWS.Cells[count, f++].Value = stats.EvToEbitda;
                            compInfoWS.Cells[count, f++].Value = stats.QuarterlyRevenueGrowth;
                            compInfoWS.Cells[count, f++].Value = stats.QuarterlyEarningsGrowth;
                            compInfoWS.Cells[count, f++].Value = stats.NetIncomeToCommonShares;
                            compInfoWS.Cells[count, f++].Value = stats.TotalCash;
                            compInfoWS.Cells[count, f++].Value = stats.CurrentRatio;
                            compInfoWS.Cells[count, f++].Value = stats.OperatingCashFlow;
                            compInfoWS.Cells[count, f++].Value = stats.LeveredCashFlow;
                            compInfoWS.Cells[count, f++].Value = stats.Roic();
                            compInfoWS.Cells[count, f++].Value = inc.Ebit;
                            compInfoWS.Cells[count, f++].Value = cvs.NopDcfsAvgGrowth.GetAverage("StockValue");
                            compInfoWS.Cells[count, f++].Value = cvs.NopDcfsAvgGrowth.GetStandardDeviation("StockValue");
                            compInfoWS.Cells[count, f++].Value = string.Format("={0}/{1}", compInfoWS.Cells[count, 43].Address, compInfoWS.Cells[count, 44].Address);
                            compInfoWS.Cells[count, f++].Value = cvs.NopDcfsDecayGrowth.GetAverage("StockValue");
                            compInfoWS.Cells[count, f++].Value = cvs.NopDcfsDecayGrowth.GetStandardDeviation("StockValue");
                            compInfoWS.Cells[count, f++].Value = string.Format("={0}/{1}", compInfoWS.Cells[count, 46].Address, compInfoWS.Cells[count, 47].Address);
                            compInfoWS.Cells[count, f++].Value = cvs.NopDcfsNoGrowth.GetAverage("StockValue");
                            compInfoWS.Cells[count, f++].Value = cvs.NopDcfsNoGrowth.GetStandardDeviation("StockValue");
                            compInfoWS.Cells[count, f++].Value = string.Format("={0}/{1}", compInfoWS.Cells[count, 49].Address, compInfoWS.Cells[count, 50].Address);
                            compInfoWS.Cells[count, f++].Value = cvs.NetIncomeDcfsAvgGrowth.GetAverage("StockValue");
                            compInfoWS.Cells[count, f++].Value = cvs.NetIncomeDcfsAvgGrowth.GetStandardDeviation("StockValue");
                            compInfoWS.Cells[count, f++].Value = string.Format("={0}/{1}", compInfoWS.Cells[count, 52].Address, compInfoWS.Cells[count, 53].Address);
                            compInfoWS.Cells[count, f++].Value = cvs.NetIncomeDcfsDecayGrowth.GetAverage("StockValue");
                            compInfoWS.Cells[count, f++].Value = cvs.NetIncomeDcfsDecayGrowth.GetStandardDeviation("StockValue");
                            compInfoWS.Cells[count, f++].Value = string.Format("={0}/{1}", compInfoWS.Cells[count, 55].Address, compInfoWS.Cells[count, 56].Address);
                            compInfoWS.Cells[count, f++].Value = cvs.NetIncomeDcfsNoGrowth.GetAverage("StockValue");
                            compInfoWS.Cells[count, f++].Value = cvs.NetIncomeDcfsNoGrowth.GetStandardDeviation("StockValue");
                            compInfoWS.Cells[count, f++].Value = string.Format("={0}/{1}", compInfoWS.Cells[count, 58].Address, compInfoWS.Cells[count, 59].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 6].Address, compInfoWS.Cells[count, 5].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 43].Address, compInfoWS.Cells[count, 5].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 46].Address, compInfoWS.Cells[count, 5].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 49].Address, compInfoWS.Cells[count, 5].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 52].Address, compInfoWS.Cells[count, 5].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 55].Address, compInfoWS.Cells[count, 5].Address);
                            compInfoWS.Cells[count, f++].Formula = string.Format("= {0} - {1}", compInfoWS.Cells[count, 58].Address, compInfoWS.Cells[count, 5].Address);

                            count++;
                            Console.WriteLine("loaded: {0}", comp.Symbol);
                        }
                        catch (Exception ex)
                        { Console.WriteLine(string.Format("ERROR: DID NOT LOAD: {0} {1}", comp.Symbol, ex.Message)); }
                    //}
                }
                
                xlPackage.Save();
            }
        }

        public void CreateAllCompaniesExcel()
        {
            string filename = @"C:\Users\Daniel\Documents\Visual Studio 2012\ExcelFiles\test_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + ".xlsx";

            FileInfo newFile = new FileInfo(filename);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                string[] compInfoColumns = CompanyInfoColumns();
                string[] valuationColumns = ValuationColumns();
                string[] financialColumns = FinancialStatisticsColumns();

                ExcelWorksheet compInfoWS = xlPackage.Workbook.Worksheets.Add("Company Info");

                int i = 1;
                compInfoWS.Row(1).Style.Font.Bold = true;
                foreach (string col in compInfoColumns)
                {
                    compInfoWS.Cells[1, i].Value = col;
                    i++;
                }

                ExcelWorksheet NopValuationWS = xlPackage.Workbook.Worksheets.Add("NopValuations");

                i = 1;
                NopValuationWS.Row(1).Style.Font.Bold = true;
                foreach (string col in valuationColumns)
                {
                    NopValuationWS.Cells[1, i].Value = col;
                    i++;
                }

                ExcelWorksheet NiValuationWS = xlPackage.Workbook.Worksheets.Add("NetIncomeValuations");
                
                i = 1;
                NiValuationWS.Row(1).Style.Font.Bold = true;
                foreach (string col in valuationColumns)
                {
                    NiValuationWS.Cells[1, i].Value = col;
                    i++;
                }

                ExcelWorksheet financialStatsWS = xlPackage.Workbook.Worksheets.Add("Financial Statistics");

                i = 1;
                financialStatsWS.Row(1).Style.Font.Bold = true;
                foreach (string col in financialColumns)
                {
                    financialStatsWS.Cells[1, i].Value = col;
                    i++;
                }

                CompanyCollection comps = CompanyBL.Instance.GetCompanies();

                CompanyValuationStatisticsCollection valuations = new CompanyValuationStatisticsCollection();
                CompanyFinancialStatisticsCollection statistics = new CompanyFinancialStatisticsCollection();
                CompanyValuationStatistics cvs;

                int count = 2;

                foreach (Company comp in comps)
                {
                    try
                    {
                        cvs = GetCompanyValuationStatistics(comp);
                        valuations.Add(cvs);
                        statistics.Add(GetFinancialStatistics(cvs));

                        compInfoWS.Cells[count, 1].Value = comp.Symbol;
                        compInfoWS.Cells[count, 2].Value = comp.CompanyName;
                        compInfoWS.Cells[count, 3].Value = comp.Industry;
                        compInfoWS.Cells[count, 4].Value = comp.Sector;

                        count++;
                        Console.WriteLine("loaded: {0}", comp.Symbol);
                    }
                    catch (Exception ex)
                    { Console.WriteLine(string.Format("ERROR: DID NOT LOAD: {0} {1}", comp.Symbol, ex.Message)); }
                }

                count = 2;
                int counterTwo = 2;
                foreach (CompanyValuationStatistics valuation in valuations)
                {
                    foreach (DiscountedCashFlow dcf in valuation.NopDcfsDecayGrowth)
                    {
                        NopValuationWS.Cells[count, 1].Value = dcf.Symbol;
                        NopValuationWS.Cells[count, 2].Value = dcf.Wacc;
                        NopValuationWS.Cells[count, 3].Value = dcf.TerminalGrowth;
                        NopValuationWS.Cells[count, 4].Value = dcf.StockValue;
                        NopValuationWS.Cells[count, 5].Value = dcf.EnterpriseValue;
                        NopValuationWS.Cells[count, 6].Value = dcf.EquityValue;
                        NopValuationWS.Cells[count, 7].Value = dcf.TerminalValue;
                        count++;
                    }

                  
                    foreach (DiscountedCashFlow dcf in valuation.NetIncomeDcfsAvgGrowth)
                    {
                        NiValuationWS.Cells[counterTwo, 1].Value = dcf.Symbol;
                        NiValuationWS.Cells[counterTwo, 2].Value = dcf.Wacc;
                        NiValuationWS.Cells[counterTwo, 3].Value = dcf.TerminalGrowth;
                        NiValuationWS.Cells[counterTwo, 4].Value = dcf.StockValue;
                        NiValuationWS.Cells[counterTwo, 5].Value = dcf.EnterpriseValue;
                        NiValuationWS.Cells[counterTwo, 6].Value = dcf.EquityValue;
                        NiValuationWS.Cells[counterTwo, 7].Value = dcf.TerminalValue;
                        counterTwo++;
                    }

                }

                count = 2;
                foreach (CompanyFinancialStatistics cfs in statistics)
                {
                    financialStatsWS.Cells[count, 1].Value = cfs.Symbol;
                    financialStatsWS.Cells[count, 2].Value = cfs.StockPrice;
                    financialStatsWS.Cells[count, 3].Value = cfs.BookValuePerShare;
                    financialStatsWS.Cells[count, 4].Value = cfs.ReturnOnEquity;
                    financialStatsWS.Cells[count, 5].Value = cfs.ReturnOnAssets;
                    financialStatsWS.Cells[count, 6].Value = cfs.DebtToEquity;
                    financialStatsWS.Cells[count, 7].Value = cfs.Roic();
                    financialStatsWS.Cells[count, 8].Value = cfs.YearHigh;
                    financialStatsWS.Cells[count, 9].Value = cfs.YearLow;
                    //financialStatsWS.Cells[count, 10].Value = cfs.OneYrTargetEstimate;
                    financialStatsWS.Cells[count, 11].Value = cfs.Eps;
                    financialStatsWS.Cells[count, 12].Value = cfs.RevenuePerShare;
                    financialStatsWS.Cells[count, 13].Value = cfs.AverageVolume;
                    financialStatsWS.Cells[count, 14].Value = cfs.MarketCap;
                    financialStatsWS.Cells[count, 15].Value = cfs.TotalDebt;
                    //financialStatsWS.Cells[count, 16].Value = cfs.ShortRatio;
                    financialStatsWS.Cells[count, 17].Value = cfs.DividendYieldPercent;
                    financialStatsWS.Cells[count, 18].Value = cfs.OperatingMargin;
                    financialStatsWS.Cells[count, 19].Value = cfs.ProfitMargin;
                    //financialStatsWS.Cells[count, 20].Value = cfs.PeRatio;

                    count++;
                }

                xlPackage.Save();
            }
        }

        private CompanyFinancialStatistics GetFinancialStatistics(CompanyValuationStatistics cvs)
        {
            CompanyFinancialStatistics cfs = new CompanyFinancialStatistics(cvs.Symbol, new DateTime());
            FSFMngr().PopulateCompanyFinancialStatisticsFromStatements(cfs, cvs.AvgGrowthProjections.Find(cvs.Year), 
                BalanceSheetBL.Instance.GetBalanceSheet(cvs.Symbol, cvs.Year), 
                CompanyAnnualDataBL.Instance.GetCompanyAnnual(cvs.Symbol, cvs.Year));
            //FSFMngr().PopulateCompanyFinancialStatisticsFromYahooApi(cfs);

            return cfs;
        }

        private CompanyValuationStatistics GetCompanyValuationStatistics(Company comp)
        {
            IncomeStatementProjectionBL incProjectionBL = new IncomeStatementProjectionBL();

            CompanyValuationStatistics cvs = new CompanyValuationStatistics();
            cvs.Symbol = comp.Symbol;
            cvs.Year = IncomeStatementBL.Instance.GetLastYear(comp.Symbol);
            cvs.NoOfYears = 5;
            cvs.CompanyName = comp.CompanyName;
            cvs.NoGrowthProjections = incProjectionBL.ProjectIncomeStatementsNoRevenueGrowth(comp.Symbol, cvs.Year, cvs.NoOfYears);
            cvs.AvgGrowthProjections = incProjectionBL.ProjectIncomeStatementsAvgRevenueGrowth(comp.Symbol, cvs.Year, cvs.NoOfYears);
            cvs.DecayGrowthProjections = incProjectionBL.ProjectIncomeStatementsDecayRevenueGrowth(comp.Symbol, cvs.Year, cvs.NoOfYears, (decimal)-0.05);
            DCFMngr().GetDiscountedCashFlows(cvs);

            return cvs;
        }

        private string[] CompanyInfoColumns()
        {
            string[] columns = new string[] { "Symbol",
                "Name",
                "Industry",
                "Sector",
            };

            return columns;
        }

        private string[] ValuationColumns()
        {
            string[] columns = new string[] { "Symbol",
                "Wacc",
                "TerminalGrowth",
                "Type",
                "StockValue",
                "EnterpriseValue",
                "EquityValue",
                "TerminalValue"
            };

            return columns;
        }

        private string[] FinancialStatisticsColumns()
        {
            string[] columns = new string[] { "Symbol",
                "StockPrice",
                "BookValuePerShare",
                "ROE",
                "ROA",
                "DebtToEquity",
                "RoIC",
                "YearHigh",
                "YearLow",
                "OneYrTargetEstimate",
                "EPS",
                "RevenuePerShare",
                "AverageVolume",
                "MarketCap",
                "TotalDebt",
                "ShortRatio",
                "DividendYieldPercent",
                "OperatingMargin",
                "ProfitMargin",
                "PERatio"
            };

            return columns;
        }

        private string[] AbrreviatedColumns()
        {
            string[] columns = new string[] { "Symbol",
                "Name",
                "Industry",
                "Sector",
                "StockPrice",
                "BookValuePerShare",
                "ROE",
                "ROA",
                "DebtToEquity",
                "YearHigh",
                "YearLow",
                "EPS",
                "RevenuePerShare",
                "AverageVolume",
                "MarketCap",
                "TotalDebt",
                "CurrentSharesShort",
                "LastMonthSharesShort",
                "%CurrentSharesShort",
                "%LastMonthSharesShort",
                "50MA",
                "200MA",
                "DividendYieldPercent",
                "OperatingMargin",
                "ProfitMargin",
                "TrailingPE",
                "ForwardPE",
                "PegRatio",
                "EnterpriseValue",
                "Price/Sales",
                "Price/Book",
                "EV/Rev",
                "EV/EBITDA",
                "Quar. Rev Growth",
                "Quar. Earnings Growth",
                "NItoCommonShares",
                "Total Cash",
                "CurrentRatio",
                "OpCashFlow",
                "LeveredCashFlow",
                "RoIC",
                "Calc Ebit",
                "NopAvg",
                "NopAvgStdDev",
                "NopAvgSharp",
                "NopDecay",
                "NopDecayStdDev",
                "NopDecaySharp",
                "NopNo",
                "NopNoStdDev",
                "NopNoSharp",
                "NiAvg",
                "NiAvgStdDev",
                "NiAvgSharp",
                "NiDecay",
                "NiDecayStdDev",
                "NiDecaySharp",
                "NiNo",
                "NiNoStdDev",
                "NiNoSharp",
                "BookValue Diff",
                "NopAvg Diff",
                "NopDecay Diff",
                "NopNo Diff",
                "NiAvg Diff",
                "NiDecay Diff",
                "NiNo Diff",
            };

            return columns;
        }

        private DiscountedCashFlowBL DCFMngr()
        {
            return _dcfMngr != null ? _dcfMngr: new DiscountedCashFlowBL();
        }

        private CompanyFinancialStatisticsBL FSFMngr()
        {
            return _fsMngr != null ? _fsMngr : new CompanyFinancialStatisticsBL();
        }

    }
}
