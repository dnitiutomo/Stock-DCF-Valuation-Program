using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaasOne;
using MaasOne.Base;
using MaasOne.Finance.YahooFinance;
using HtmlAgilityPack;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL
{
    public class YahooApiGateway
    {
        public Dictionary<string, Object> GetCompanyAnnualStats(string symbol)
        {
            Dictionary<string, Object> stats = new Dictionary<string, Object>();
            RetrieveCompanyStatsFromYahooWebPage(symbol, ref stats);
            RetrieveYahooCompanyInfo(symbol, ref stats);
            RetrieveQuoteProperties(symbol, ref stats);

            return stats;
        }

        public Dictionary<string, Object> GetAllCompanyStatsFromYahoo(string symbol)
        {
            Dictionary<string, Object> stats = new Dictionary<string, Object>();
            RetrieveQuoteProperties(symbol, ref stats);
            RetrieveYahooCompanyStatistics(symbol, ref stats);
            RetrieveYahooCompanyProfile(symbol, ref stats);

            return stats;
        }

        private void RetrieveQuoteProperties(string symbol, ref Dictionary<string, Object> stats)
        {
            QuotesDownload dl = new QuotesDownload();
            DownloadClient<QuotesResult> baseDl = dl;

            QuotesDownloadSettings settings = dl.Settings;
            settings.IDs = new string[] { symbol };
            settings.Properties = new QuoteProperty[] {
                QuoteProperty.Symbol,
                QuoteProperty.Name,
                QuoteProperty.LastTradePriceOnly,
                QuoteProperty.MarketCapitalization,
                QuoteProperty.PERatio,
                QuoteProperty.EBITDA,
                QuoteProperty.PEGRatio,
                QuoteProperty.Revenue,
                QuoteProperty.YearHigh,
                QuoteProperty.YearLow,
                QuoteProperty.ShortRatio,
                QuoteProperty.OneyrTargetPrice,
                QuoteProperty.AverageDailyVolume,
                QuoteProperty.BookValuePerShare,
                QuoteProperty.DilutedEPS,
                //QuoteProperty.TrailingAnnualDividendYield,
                QuoteProperty.TrailingAnnualDividendYieldInPercent,
                //QuoteProperty.SharesOutstanding
            };

            Response<QuotesResult> resp = baseDl.Download();
            SettingsBase baseSettings = baseDl.Settings;

            ConnectionInfo connInfo = resp.Connection;
            if (connInfo.State == ConnectionState.Success)
            {
                QuotesResult result = resp.Result;

                stats.Add("LastTradePriceOnly", CheckDecimalItem(Convert.ToString(result.Items[0].LastTradePriceOnly)));
                stats.Add("AverageDailyVolume", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.AverageDailyVolume))));
                stats.Add("MarketCapitalization", GetDecimalFromConString(Convert.ToString(result.Items[0].Values(QuoteProperty.MarketCapitalization))));
                stats.Add("EBITDA", GetDecimalFromConString(Convert.ToString(result.Items[0].Values(QuoteProperty.EBITDA))));
                stats.Add("PEGRatio", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.PEGRatio))));
                stats.Add("Revenue", GetDecimalFromConString(Convert.ToString(result.Items[0].Values(QuoteProperty.Revenue))));
                stats.Add("PERatio", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.PERatio))));
                stats.Add("YearHigh", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.YearHigh))));
                stats.Add("YearLow", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.YearLow))));
                stats.Add("ShortRatio", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.ShortRatio))));
                stats.Add("OneyrTargetPrice", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.OneyrTargetPrice))));
                stats.Add("BookValuePerShare", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.BookValuePerShare))));
                stats.Add("DilutedEPS", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.DilutedEPS))));
                stats.Add("Name", Convert.ToString(result.Items[0].Name));

                stats.Add("TrailingAnnualDividendYieldInPercent", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.TrailingAnnualDividendYieldInPercent))));
                //stats.Add("TrailingAnnualDividendYield", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.TrailingAnnualDividendYield))));
                //stats.Add("SharesOutstanding", CheckDecimalItem(Convert.ToString(result.Items[0].Values(QuoteProperty.SharesOutstanding))));
            }
            else
            {
                Exception ex = connInfo.Exception;
                Console.WriteLine(ex.Message);
            }
        }

        private void RetrieveYahooCompanyStatistics(string symbol, ref Dictionary<string, Object> stats)
        {

            CompanyStatisticsDownload d2 = new CompanyStatisticsDownload();
            DownloadClient<CompanyStatisticsResult> baseD2 = d2;

            CompanyStatisticsDownloadSettings settings2 = d2.Settings;
            settings2.ID = symbol;

            Response<CompanyStatisticsResult> resp2 = baseD2.Download();
            SettingsBase baseSettings = baseD2.Settings;

            ConnectionInfo connInfo = resp2.Connection;
            if (connInfo.State == ConnectionState.Success)
            {
                CompanyStatisticsResult result = resp2.Result;
                CompanyFinancialHighlights highlights = result.Item.FinancialHighlights;
                stats.Add("TotalDeptPerEquity", CheckDecimalItem(Convert.ToString(highlights.TotalDeptPerEquity)));
                stats.Add("OperatingMarginPercent", CheckDecimalItem(Convert.ToString(highlights.OperatingMarginPercent)));
                stats.Add("ProfitMarginPercent", CheckDecimalItem(Convert.ToString(highlights.ProfitMarginPercent)));
                stats.Add("ReturnOnAssetsPercent", CheckDecimalItem(Convert.ToString(highlights.ReturnOnAssetsPercent)));
                stats.Add("ReturnOnEquityPercent", CheckDecimalItem(Convert.ToString(highlights.ReturnOnEquityPercent)));
                stats.Add("TotalDeptInMillion", CheckDecimalItem(Convert.ToString(highlights.TotalDeptInMillion)) * 1000000);
                stats.Add("EBITDAInMillion", CheckDecimalItem(Convert.ToString(highlights.EBITDAInMillion)) * 1000000);
                stats.Add("CurrentRatio", CheckDecimalItem(Convert.ToString(highlights.CurrentRatio)));
                stats.Add("GrossProfitInMillion", CheckDecimalItem(Convert.ToString(highlights.GrossProfitInMillion)) * 1000000);
                stats.Add("LeveredFreeCashFlowInMillion", CheckDecimalItem(Convert.ToString(highlights.LeveredFreeCashFlowInMillion)) * 1000000);
                stats.Add("OperatingCashFlowInMillion", CheckDecimalItem(Convert.ToString(highlights.OperatingCashFlowInMillion)) * 1000000);
                stats.Add("QuarterlyRevenueGrowthPercent", CheckDecimalItem(Convert.ToString(highlights.QuarterlyRevenueGrowthPercent)));
                stats.Add("QuarterlyEarningsGrowthPercent", CheckDecimalItem(Convert.ToString(highlights.QuaterlyEarningsGrowthPercent)));
                stats.Add("RevenuePerShare", CheckDecimalItem(Convert.ToString(highlights.RevenuePerShare)));
                stats.Add("TotalCashInMillion", CheckDecimalItem(Convert.ToString(highlights.TotalCashInMillion)) * 1000000);
                stats.Add("TotalCashPerShare", CheckDecimalItem(Convert.ToString(highlights.TotalCashPerShare)));;
            }
            else
            {
                Exception ex = connInfo.Exception;
                Console.WriteLine(ex.Message);
            }
        }

        public void RetrieveYahooCompanyInfo(string symbol, ref Dictionary<string, Object> stats)
        {
            CompanyInfoDownload d1 = new CompanyInfoDownload();
            DownloadClient<CompanyInfoResult> baseD1 = d1;

            CompanyInfoDownloadSettings settings = d1.Settings;
            settings.IDs = new string[] { symbol };

            Response<CompanyInfoResult> resp = baseD1.Download();
            SettingsBase baseSettings = baseD1.Settings;

            ConnectionInfo connInfo = resp.Connection;
            if (connInfo.State == ConnectionState.Success)
            {
                CompanyInfoResult result = resp.Result;
                
                stats.Add("Sector", result.Items[0].SectorName);
                stats.Add("Industry", result.Items[0].IndustryName);
            }
            else
            {
                Exception ex = connInfo.Exception;
                Console.WriteLine(ex.Message);
            }

            QuotesDownload d2 = new QuotesDownload();
            DownloadClient<QuotesResult> baseD2 = d2;

            QuotesDownloadSettings settings1 = d2.Settings;
            settings1.IDs = new string[] { symbol };
            settings1.Properties = new QuoteProperty[] {
                QuoteProperty.Symbol,
                QuoteProperty.Name
           };

            Response<QuotesResult> resp1 = baseD2.Download();
            SettingsBase baseSettings1 = baseD2.Settings;

            ConnectionInfo connInfo1 = resp1.Connection;
            if (connInfo1.State == ConnectionState.Success)
            {
                QuotesResult result = resp1.Result;
                stats.Add("Name", Convert.ToString(result.Items[0].Name));
            }
            else
            {
                Exception ex = connInfo.Exception;
                Console.WriteLine(ex.Message);
            }
        }

        private void RetrieveYahooCompanyProfile(string symbol, ref Dictionary<string, Object> stats)
        {
            CompanyProfileDownload d1 = new CompanyProfileDownload();
            DownloadClient<CompanyProfileResult> baseD1 = d1;

            CompanyProfileDownloadSettings settings = d1.Settings;
            settings.ID = symbol;

            Response<CompanyProfileResult> resp = baseD1.Download();
            SettingsBase baseSettings = baseD1.Settings;

            ConnectionInfo connInfo = resp.Connection;
            if (connInfo.State == ConnectionState.Success)
            {
                CompanyProfileResult result = resp.Result;
                //stats.Add("CorporateGovernance", result.Item.CorporateGovernance);
                stats.Add("BusinessSummary", result.Item.BusinessSummary);

            }
            else
            {
                Exception ex = connInfo.Exception;
                Console.WriteLine(ex.Message);
            }
        }

        private void RetrieveCompanyStatsFromYahooWebPage(string symbol, ref Dictionary<string, Object> stats)
        {
            string url = "http://finance.yahoo.com/q?s=" + symbol + "&ql=1";
            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//th[@scope='row' and @width='48%']"))
            {
                if (div.InnerHtml.Contains("Beta")) 
                {
                    stats.Add("Beta" , Convert.ToDecimal(div.NextSibling.InnerHtml));
                }
            }
        }

        private decimal GetDividend(String div)
        {
            if (!div.Contains("N/A"))
            {
              return Convert.ToDecimal(div.Split(new[] { '(' })[0]);
            }
            else
            {
                return 0;
            }
        }

        private decimal GetDecimalFromConString(string obj)
        {
            decimal marketCap = 0;
            string intermediary;
            if (obj.Contains("B"))
            {
                intermediary = obj.Split(new[] { 'B' })[0];
                marketCap = Convert.ToDecimal(intermediary) * 1000000000;
            }
            else if (obj.Contains("M"))
            {
                intermediary = obj.Split(new[] { 'M' })[0];
                marketCap = Convert.ToDecimal(intermediary) * 1000000;
            }
            

            return marketCap;
        }

        private decimal CheckDecimalItem(string obj)
        {
            if (obj.Equals(""))
            {
                return 0;
            }
            else if (obj.Equals("0.0"))
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(obj);
            }
        }
    }
}
