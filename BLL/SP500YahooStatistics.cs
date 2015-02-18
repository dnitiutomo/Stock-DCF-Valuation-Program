using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using OfficeOpenXml;

namespace StockValuationLibrary._2.BLL
{
    public class SP500YahooStatistics
    {
        public void GetSP500Components()
        {
            string url = "http://en.wikipedia.org/wiki/List_of_S%26P_500_companies";
            HtmlDocument doc = new HtmlWeb().Load(url);
            List<string> companies = new List<string>();

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//a[@href and @rel='nofollow' and @class='external text']"))
            {
                if (div.Attributes["href"].Value.Contains("ticker"))
                {
                    companies.Add(div.InnerHtml);
                }
            }

            CreateExcelDocument(companies);
        }

        private void CreateExcelDocument(List<string> comps)
        {
            string filename = @"C:\Users\Daniel\Documents\Visual Studio 2012\ExcelFiles\yahooStatsTest_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + ".xlsx";
            FileInfo newFile = new FileInfo(filename);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                string[] financialColumns = FinancialStatisticsColumns();
                ExcelWorksheet finStatsWS = xlPackage.Workbook.Worksheets.Add("FinancialStatistics");

                int i = 1;
                finStatsWS.Row(1).Style.Font.Bold = true;
                foreach (string col in financialColumns)
                {
                    finStatsWS.Cells[1, i].Value = col;
                    i++;
                }

                YahooApiGateway yahooAPI = new YahooApiGateway();

                int count = 2;
                foreach (string company in comps)
                {
                    if (!company.Contains("."))
                    {
                        Dictionary<string, Object> compStats = yahooAPI.GetAllCompanyStatsFromYahoo(company);

                        finStatsWS.Cells[count, 1].Value = company;
                        finStatsWS.Cells[count, 2].Value = compStats["Name"];
                        //finStatsWS.Cells[count, 3].Value = compStats["Sector"];
                        //finStatsWS.Cells[count, 4].Value = compStats["Industry"];
                        finStatsWS.Cells[count, 5].Value = compStats["BusinessSummary"];
                        finStatsWS.Cells[count, 6].Value = compStats["LastTradePriceOnly"];
                        finStatsWS.Cells[count, 7].Value = compStats["BookValuePerShare"];
                        finStatsWS.Cells[count, 8].Value = compStats["ReturnOnEquityPercent"];
                        finStatsWS.Cells[count, 9].Value = compStats["ReturnOnAssetsPercent"];
                        finStatsWS.Cells[count, 10].Value = compStats["TotalDeptPerEquity"];
                        finStatsWS.Cells[count, 11].Value = compStats["YearHigh"];
                        finStatsWS.Cells[count, 12].Value = compStats["YearLow"];
                        finStatsWS.Cells[count, 13].Value = compStats["OneyrTargetPrice"];
                        finStatsWS.Cells[count, 14].Value = compStats["DilutedEPS"];
                        finStatsWS.Cells[count, 15].Value = compStats["RevenuePerShare"];
                        finStatsWS.Cells[count, 16].Value = compStats["AverageDailyVolume"];
                        finStatsWS.Cells[count, 17].Value = compStats["MarketCapitalization"];
                        finStatsWS.Cells[count, 18].Value = compStats["TotalDeptInMillion"];
                        finStatsWS.Cells[count, 19].Value = compStats["ShortRatio"];
                        finStatsWS.Cells[count, 20].Value = compStats["FiftydayMovingAverage"];
                        finStatsWS.Cells[count, 21].Value = compStats["TwoHundreddayMovingAverage"];
                        finStatsWS.Cells[count, 22].Value = compStats["TrailingAnnualDividendYieldInPercent"];
                        finStatsWS.Cells[count, 23].Value = compStats["OperatingMarginPercent"];
                        finStatsWS.Cells[count, 24].Value = compStats["ProfitMarginPercent"];
                        finStatsWS.Cells[count, 25].Value = compStats["PERatio"];
                        finStatsWS.Cells[count, 26].Value = compStats["PEGRatio"];
                        finStatsWS.Cells[count, 27].Value = compStats["EBITDA"];
                        finStatsWS.Cells[count, 28].Value = compStats["Revenue"];
                        //finStatsWS.Cells[count, 29].Value = compStats["SharesOutstanding"];
                        finStatsWS.Cells[count, 30].Value = compStats["EBITDAInMillion"];
                        finStatsWS.Cells[count, 31].Value = compStats["CurrentRatio"];
                        finStatsWS.Cells[count, 32].Value = compStats["GrossProfitInMillion"];
                        finStatsWS.Cells[count, 33].Value = compStats["LeveredFreeCashFlowInMillion"];
                        finStatsWS.Cells[count, 34].Value = compStats["OperatingCashFlowInMillion"];
                        finStatsWS.Cells[count, 35].Value = compStats["QuarterlyRevenueGrowthPercent"];
                        finStatsWS.Cells[count, 36].Value = compStats["QuarterlyEarningsGrowthPercent"];
                        finStatsWS.Cells[count, 37].Value = compStats["RevenuePerShare"];
                        finStatsWS.Cells[count, 38].Value = compStats["TotalCashInMillion"];
                        finStatsWS.Cells[count, 39].Value = compStats["TotalCashPerShare"];

                        count++;
                    }
                } 
                xlPackage.Save();
            }
        }

        private string[] FinancialStatisticsColumns()
        {
            string[] columns = new string[] { "Symbol",
                "Name",
                "Industry",
                "Sector",
                "BusinessSummary",
                "LastTradePrice",
                "BookValuePerShare",
                "ROE",
                "ROA",
                "DebtToEquity",
                "YearHigh",
                "YearLow",
                "OneYrTargetEstimate",
                "EPS",
                "RevenuePerShare",
                "AverageVolume",
                "MarketCap",
                "TotalDebt",
                "ShortRatio",
                "FiftyMovingAverage",
                "TwoHundredMovingAverage",
                "DividendYieldPercent",
                "OperatingMargin",
                "ProfitMargin",
                "PERatio",
                "PEGRatio",
                "EBITDA",
                "Revenue",
                "SharesOutstanding",
                "EBITDAInMillion",
                "CurrentRatio",
                "GrossProfitInMillion",
                "LeveredFreeCashFlow",
                "OperatingCashFlow",
                "QuarterlyRevenueGrowthPercent",
                "QuarterlyEarningsGrowthPercent",
                "RevenuePerShare",
                "TotalCashInMillion",
                "TotalCashPerShare"
            };

            return columns;
        }

    }
}
