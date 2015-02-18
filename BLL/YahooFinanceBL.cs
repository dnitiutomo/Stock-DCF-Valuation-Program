using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;
using HtmlAgilityPack;

namespace StockValuationLibrary._2.BLL
{
    public class YahooFinanceBL
    {
        public void EnterAllCompaniesStatsFromYahooFinance()
        {
            CompanyCollection companies = CompanyBL.Instance.GetCompanies();

            foreach (Company comp in companies)
            {
                try
                {
                    AddCompanyToDatabase(comp.Symbol, 2012, 3);
                    Console.WriteLine("Successfully Loaded {0}", comp.Symbol);
                }
                catch (Exception ex)
                {
                    { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", comp.Symbol, ex.Message)); }
                }
            }
        }
        
        public void LoadSP500Components()
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

            foreach (string symbol in companies)
            {
                if (!CompanyBL.Instance.CompanyExists(symbol))
                {
                    try
                    {
                        AddCompanyToDatabase(symbol, 2012, 3);
                        Console.WriteLine("Successfully Loaded {0}", symbol);
                    }
                    catch (Exception ex)
                    {
                        { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", symbol, ex.Message)); }
                    }
                }
            }
        }

        public void LoadNasdaq100Companies()
        {
            string filename = @"C:\Users\Daniel\Documents\Visual Studio 2012\ExcelFiles\companylist.csv";   
            string[] allLines = File.ReadAllLines(filename);

            foreach (string line in allLines)
            {
                string[] companyArray = line.Split(',');

                string symbol = companyArray[0].Substring(1, companyArray[0].Length - 2);

                if (!CompanyBL.Instance.CompanyExists(symbol))
                {
                    try
                    {
                        AddCompanyToDatabase(symbol, 2012, 3);
                        Console.WriteLine("Successfully Loaded {0}", symbol);
                    }
                    catch (Exception ex)
                    {
                        { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", symbol, ex.Message)); }
                    }
                }
            }
        }

        public void LoadNyseComponents()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                string url = "http://en.wikipedia.org/wiki/Companies_listed_on_the_New_York_Stock_Exchange_(" + c + ")";
                HtmlDocument doc = new HtmlWeb().Load(url);
                List<string> companies = new List<string>();

                foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//a[@href and @rel='nofollow' and @class='external text']"))
                {
                    if (div.Attributes["href"].Value.Contains("ticker=") && div.ParentNode.NextSibling.NextSibling.LastChild.InnerHtml.Equals("USA"))
                    {
                        if (div.InnerHtml.Length < 5 && !div.InnerHtml.Contains("."))
                        {
                            if (!CompanyBL.Instance.CompanyExists(div.InnerHtml))
                            {
                                try
                                {
                                    AddCompanyToDatabase(div.InnerHtml, 2012, 3);
                                    Console.WriteLine("Successfully Loaded {0}", div.InnerHtml);
                                }
                                catch (Exception ex)
                                {
                                    { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", div.InnerHtml, ex.Message)); }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AddCompanyToDatabase(string symbol, int year, int yearsToLoad)
        {
            CompanyBL.Instance.UpdateCompany(YahooHtmlParser.Instance.GetCompanyProfile(symbol));      

            IncomeStatementCollection incs = YahooHtmlParser.Instance.GetIncomeStatementData(symbol, year, yearsToLoad);
            BalanceSheetCollection bals = YahooHtmlParser.Instance.GetBalanceSheetData(symbol, year, yearsToLoad);
            CompanyAnnualData compAn = YahooHtmlParser.Instance.GetCompanyAnnualData(symbol, year);
            YahooHtmlParser.Instance.GetCashFlowStatementData(symbol, year, yearsToLoad, incs);

            EnterFinancialData(incs, bals, compAn, year, yearsToLoad);
        }

        private void EnterIntoCompanyTableFromYahooApi(string symbol)
        {
            YahooApiGateway api = new YahooApiGateway();
            Dictionary<string, Object> stats = new Dictionary<string, object>();

            api.RetrieveYahooCompanyInfo(symbol, ref stats);

            Company comp = new Company(symbol, Convert.ToString(stats["Name"]), Convert.ToString(stats["Industry"]), Convert.ToString(stats["Sector"]));

            CompanyBL.Instance.UpdateCompany(comp);
        }

        private void EnterFinancialData(IncomeStatementCollection incs, BalanceSheetCollection bals, CompanyAnnualData cad, int year, int yearsToLoad)
        {
            CompanyAnnualDataBL.Instance.UpdateCompanyAnnual(cad);

            for (int y = year; y > year - yearsToLoad; y--)
            {
                IncomeStatementBL.Instance.UpdateIncomeStatement(incs.Find(y));
                BalanceSheetBL.Instance.UpdateBalanceSheet(bals.Find(y));
            }   
        }
    }
}
