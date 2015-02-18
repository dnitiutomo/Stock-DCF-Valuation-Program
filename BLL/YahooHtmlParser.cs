using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL
{
    public class YahooHtmlParser
    {
        private static YahooHtmlParser instance;

        private YahooHtmlParser() { }

        public static YahooHtmlParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new YahooHtmlParser();
                }
                return instance;
            }
        }

        public IncomeStatementCollection GetIncomeStatementData(string symbol, int year, int yearsToLoad)
        {
            string url = "http://finance.yahoo.com/q/is?s=" + symbol + "+Income+Statement&annual";
            HtmlDocument doc = new HtmlWeb().Load(url);

            Dictionary<int, Dictionary<string, decimal>> values = new Dictionary<int, Dictionary<string, decimal>>();

            Dictionary<string, decimal> inc2012 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> inc2011 = new Dictionary<string,decimal>();
            Dictionary<string, decimal> inc2010 = new Dictionary<string,decimal>();

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@colspan='2']"))
            {
                if (div.InnerHtml.Contains("Total Revenue"))
                {
                    inc2012.Add("Total Revenue", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    inc2011.Add("Total Revenue", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    inc2010.Add("Total Revenue", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));

                }
                else if (div.InnerHtml.Contains("Cost of Revenue"))
                {
                    inc2012.Add("Cost of Revenue", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                    inc2011.Add("Cost of Revenue", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2010.Add("Cost of Revenue", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.InnerHtml.Contains("Net Income") && !div.InnerHtml.Contains("Ops") && !div.InnerHtml.Contains("To"))
                {
                    inc2012.Add("Net Income", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    inc2011.Add("Net Income", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    inc2010.Add("Net Income", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                }
            }

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@width ='30' and @class='yfnc_tabledata1']"))
            {
                if (div.NextSibling.NextSibling.InnerHtml.Contains("Research Development"))
                {
                    inc2012.Add("Research Development", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2011.Add("Research Development", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2010.Add("Research Development", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Selling General and Administrative"))
                {
                    inc2012.Add("Selling General and Administrative", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2011.Add("Selling General and Administrative", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2010.Add("Selling General and Administrative", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Non Recurring"))
                {
                    inc2012.Add("Non Recurring", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2011.Add("Non Recurring", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2010.Add("Non Recurring", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Others"))
                {
                    inc2012.Add("Others", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2011.Add("Others", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    inc2010.Add("Others", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Total Other Income/Expenses Net"))
                //{
                //    inc2012.Add("Total Other Income/Expenses Net", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Total Other Income/Expenses Net", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Total Other Income/Expenses Net", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Earnings Before Interest And Taxes"))
                //{
                //    inc2012.Add("Earnings Before Interest And Taxes", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Earnings Before Interest And Taxes", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Earnings Before Interest And Taxes", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Interest Expense"))
                //{
                //    inc2012.Add("Interest Expense", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Interest Expense", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Interest Expense", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Income Tax Expense"))
                //{
                //    inc2012.Add("Income Tax Expense", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Income Tax Expense", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Income Tax Expense", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Minority Interest"))
                //{
                //    inc2012.Add("Minority Interest", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Minority Interest", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Minority Interest", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Discontinued Operations"))
                //{
                //    inc2012.Add("Discontinued Operations", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Discontinued Operations", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Discontinued Operations", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Extraordinary Items"))
                //{
                //    inc2012.Add("Extraordinary Items", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Extraordinary Items", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Extraordinary Items", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Effect Of Accounting Changes"))
                //{
                //    inc2012.Add("Effect Of Accounting Changes", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Effect Of Accounting Changes", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Effect Of Accounting Changes", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.NextSibling.NextSibling.InnerHtml.Contains("Other Items"))
                //{
                //    inc2012.Add("Other Items", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2011.Add("Other Items", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    inc2010.Add("Other Items", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
            }

            values.Add(2010, inc2010);
            values.Add(2011, inc2011);
            values.Add(2012, inc2012);

            IncomeStatementCollection incs = new IncomeStatementCollection();

            for (int y = year; y > year - yearsToLoad; y--)
            {
                if (values.ContainsKey(y))
                {
                    incs.Add(new IncomeStatement(symbol, y,
                        values[y]["Total Revenue"],
                        values[y]["Cost of Revenue"],
                        values[y]["Research Development"] + values[y]["Selling General and Administrative"] + values[y]["Non Recurring"] + values[y]["Others"],
                        0,
                        values[y]["Net Income"],
                        0)
                    );
                }
            }


            return incs;
        }

        public BalanceSheetCollection GetBalanceSheetData(string symbol, int year, int yearsToLoad)
        {
            Dictionary<int, Dictionary<string, decimal>> values = new Dictionary<int, Dictionary<string, decimal>>();

            Dictionary<string, decimal> bs2012 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> bs2011 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> bs2010 = new Dictionary<string, decimal>();

            string url = "http://finance.yahoo.com/q/bs?s=" + symbol + "+Balance+Sheet&annual";
            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@width ='30' and @class='yfnc_tabledata1']"))
            {
                if (div.NextSibling.NextSibling.InnerHtml.Contains("Cash And Cash Equivalents"))
                {
                    bs2012.Add("Cash And Cash Equivalents", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Cash And Cash Equivalents", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Cash And Cash Equivalents", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Short Term Investments"))
                {
                    bs2012.Add("Short Term Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Short Term Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Short Term Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Net Receivables"))
                {
                    bs2012.Add("Net Receivables", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Net Receivables", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Net Receivables", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Inventory"))
                {
                    bs2012.Add("Inventory", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Inventory", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Inventory", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Other Current Assets"))
                {
                    bs2012.Add("Other Current Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Other Current Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Other Current Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Accounts Payable"))
                {
                    bs2012.Add("Accounts Payable", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Accounts Payable", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Accounts Payable", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Short/Current Long Term Debt"))
                {
                    bs2012.Add("Short/Current Long Term Debt", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Short/Current Long Term Debt", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Short/Current Long Term Debt", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.NextSibling.NextSibling.InnerHtml.Contains("Other Current Liabilities"))
                {
                    bs2012.Add("Other Current Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Other Current Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Other Current Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
            }

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@colspan='2']"))
            {
                //if (div.InnerHtml.Contains("Total Current Assets"))
                //{
                //    bs2012.Add("Total Current Assets", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //    bs2011.Add("Total Current Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //    bs2010.Add("Total Current Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));

                //}
                //else if (div.InnerHtml.Equals("Long Term Investments"))
                //{
                //    bs2012.Add("Long Term Investments", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Long Term Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Long Term Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                if (div.InnerHtml.Contains("Property Plant and Equipment"))
                {
                    bs2012.Add("Property Plant and Equipment", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Property Plant and Equipment", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Property Plant and Equipment", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                //else if (div.InnerHtml.Contains("Goodwill"))
                //{
                //    bs2012.Add("Goodwill", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Goodwill", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Goodwill", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Intangible Assets"))
                //{
                //    bs2012.Add("Intangible Assets", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Intangible Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Intangible Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Accumulated Amortization"))
                //{
                //    bs2012.Add("Accumulated Amortization", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Accumulated Amortization", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Accumulated Amortization", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Other Assets"))
                //{
                //    bs2012.Add("Other Assets", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Other Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Other Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Deferred Long Term Asset Charges"))
                //{
                //    bs2012.Add("Deferred Long Term Asset Charges", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Deferred Long Term Asset Charges", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Deferred Long Term Asset Charges", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                else if (div.InnerHtml.Contains("Total Assets"))
                {
                    bs2012.Add("Total Assets", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    bs2011.Add("Total Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    bs2010.Add("Total Assets", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                }
                //else if (div.InnerHtml.Contains("Total Current Liabilities"))
                //{
                //    bs2012.Add("Total Current Liabilities", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //    bs2011.Add("Total Current Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //    bs2010.Add("Total Current Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //}
                else if (div.InnerHtml.Contains("Long Term Debt"))
                {
                    bs2012.Add("Long Term Debt", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Long Term Debt", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Long Term Debt", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                //else if (div.InnerHtml.Contains("Other Liabilities"))
                //{
                //    bs2012.Add("Other Liabilities", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Other Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Other Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Deferred Long Term Liability Charges"))
                //{
                //    bs2012.Add("Deferred Long Term Liability Charges", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Deferred Long Term Liability Charges", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Deferred Long Term Liability Charges", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Minority Interest"))
                //{
                //    bs2012.Add("Minority Interest", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Minority Interest", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Minority Interest", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Negative Goodwill"))
                //{
                //    bs2012.Add("Negative Goodwill", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Negative Goodwill", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Negative Goodwill", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Total Liabilities"))
                //{
                //    bs2012.Add("Total Liabilities", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //    bs2011.Add("Total Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //    bs2010.Add("Total Liabilities", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Misc Stocks Options Warrants"))
                //{
                //    bs2012.Add("Misc Stocks Options Warrants", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Misc Stocks Options Warrants", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Misc Stocks Options Warrants", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Redeemable Preferred Stock"))
                //{
                //    bs2012.Add("Redeemable Preferred Stock", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Redeemable Preferred Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Redeemable Preferred Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Preferred Stock"))
                //{
                //    bs2012.Add("Preferred Stock", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Preferred Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Preferred Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Common Stock"))
                //{
                //    bs2012.Add("Common Stock", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Common Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Common Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Retained Earnings"))
                //{
                //    bs2012.Add("Retained Earnings", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Retained Earnings", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Retained Earnings", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Treasury Stock"))
                //{
                //    bs2012.Add("Treasury Stock", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Treasury Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Treasury Stock", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Capital Surplus"))
                //{
                //    bs2012.Add("Capital Surplus", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Capital Surplus", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Capital Surplus", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                //else if (div.InnerHtml.Contains("Other Stockholder Equity"))
                //{
                //    bs2012.Add("Other Stockholder Equity", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Other Stockholder Equity", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Other Stockholder Equity", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
                else if (div.InnerHtml.Contains("Total Stockholder Equity"))
                {
                    bs2012.Add("Total Stockholder Equity", GetValueFromInnerHtml(div.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    bs2011.Add("Total Stockholder Equity", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                    bs2010.Add("Total Stockholder Equity", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes["strong"].ChildNodes[0].InnerHtml));
                }
            }

            values.Add(2010, bs2010);
            values.Add(2011, bs2011);
            values.Add(2012, bs2012);

            BalanceSheetCollection bss = new BalanceSheetCollection();

            for (int y = year; y > year - yearsToLoad; y--)
            {
                if (values.ContainsKey(y))
                {
                    bss.Add(new BalanceSheet(symbol, y,
                        values[y]["Cash And Cash Equivalents"] + values[y]["Short Term Investments"] + values[y]["Net Receivables"] + values[y]["Inventory"] + values[y]["Other Current Assets"],
                        values[y]["Accounts Payable"] + values[y]["Short/Current Long Term Debt"] + values[y]["Other Current Liabilities"],
                        values[y]["Long Term Debt"],
                        values[y]["Property Plant and Equipment"],
                        values[y]["Cash And Cash Equivalents"],
                        values[y]["Total Stockholder Equity"],
                        values[y]["Total Assets"]
                        )
                    );
                }
            }


            return bss;
        }

        public void GetCashFlowStatementData(string symbol, int year, int yearsToLoad, IncomeStatementCollection incs)
        {
            Dictionary<int, Dictionary<string, decimal>> values = new Dictionary<int, Dictionary<string, decimal>>();

            Dictionary<string, decimal> bs2012 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> bs2011 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> bs2010 = new Dictionary<string, decimal>();

            string url = "http://finance.yahoo.com/q/cf?s=" + symbol + "+Cash+Flow&annual";
            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@colspan='2']"))
            {
                if (div.InnerHtml.Contains("Depreciation"))
                {
                    bs2012.Add("Depreciation", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                    bs2011.Add("Depreciation", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                    bs2010.Add("Depreciation", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                }
                else if (div.InnerHtml.Contains("Capital Expenditures"))
                {
                    bs2012.Add("Capital Expenditures", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml) * -1);
                    bs2011.Add("Capital Expenditures", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml) * -1);
                    bs2010.Add("Capital Expenditures", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml) * -1);
                }
                //else if (div.InnerHtml.Equals("Investments"))
                //{
                //    bs2012.Add("Investments", GetValueFromInnerHtml(div.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2011.Add("Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //    bs2010.Add("Investments", GetValueFromInnerHtml(div.NextSibling.NextSibling.NextSibling.ChildNodes[0].InnerHtml));
                //}
            }

            values.Add(2010, bs2010);
            values.Add(2011, bs2011);
            values.Add(2012, bs2012);

            for (int y = year; y > year - yearsToLoad; y--)
            {
                if (values.ContainsKey(y))
                {
                    incs.Find(y).Depreciation = values[y]["Depreciation"];
                    incs.Find(y).CapitalExpenditures = values[y]["Capital Expenditures"];
                }
            }
        }

        public CompanyAnnualData GetCompanyAnnualData(string symbol, int year)
        {
            Dictionary<string, decimal> cd2012 = new Dictionary<string, decimal>();

            string url = "http://finance.yahoo.com/q/ks?s=" + symbol + "+Key+Statistics";
            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@class='yfnc_tablehead1' and @width='74%']"))
            {
                if (div.InnerHtml.Contains("Shares Outstanding"))
                {
                    cd2012.Add("Shares Outstanding", GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml));
                }
            }

            return new CompanyAnnualData(symbol, year, 0,0, cd2012["Shares Outstanding"], 0);
        }

        public void GetCompaniesFinancialData(CompanyFinancialStatistics fin)
        {
            string url = "http://finance.yahoo.com/q/ks?s=" + fin.Symbol + "+Key+Statistics";
            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//span[@class='time_rtq_ticker']"))
            {
                fin.StockPrice = GetDecimalFromConString(div.ChildNodes[0].InnerHtml);
            }

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@class='yfnc_tablehead1' and @width='74%']"))
            {
                if (div.InnerHtml.Contains("Market Cap (intraday)"))
                {
                    fin.MarketCap = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Enterprise Value") && !div.InnerHtml.Contains("Revenue") && !div.InnerHtml.Contains("EBITDA"))
                {
                    fin.EnterpriseValue =  GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Trailing P/E"))
                {
                    fin.TrailingPE= GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Forward P/E"))
                {
                    fin.ForwardPE = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("PEG Ratio"))
                {
                    fin.PegRatio = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Price/Sales"))
                {
                    fin.PriceToSales = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Price/Book"))
                {
                    fin.PriceToBook = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Enterprise Value/Revenue"))
                {
                    fin.EvToRevenue = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Enterprise Value/EBITDA"))
                {
                    fin.EvToEbitda = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Profit Margin"))
                {
                    fin.ProfitMargin = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Operating Margin"))
                {
                    fin.OperatingMargin = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Return on Assets"))
                {
                    fin.ReturnOnAssets = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Return on Equity"))
                {
                    fin.ReturnOnEquity = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Revenue Per Share"))
                {
                    fin.RevenuePerShare = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Qtrly Revenue Growth (yoy)"))
                {
                    fin.QuarterlyRevenueGrowth = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("EBITDA (ttm)"))
                {
                    fin.Ebitda = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Net Income Avl to Common"))
                {
                    fin.NetIncomeToCommonShares = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Diluted EPS"))
                {
                    fin.Eps = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Qtrly Earnings Growth (yoy)"))
                {
                    fin.QuarterlyEarningsGrowth = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Total Cash (mrq)"))
                {
                    fin.TotalCash = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Total Debt (mrq)"))
                {
                    fin.TotalDebt = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Total Debt/Equity"))
                {
                    fin.DebtToEquity = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Current Ratio"))
                {
                    fin.CurrentRatio = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Book Value Per Share"))
                {
                    fin.BookValuePerShare = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Operating Cash Flow"))
                {
                    fin.OperatingCashFlow = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Levered Free Cash Flow"))
                {
                    fin.LeveredCashFlow = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Avg Vol (3 month)"))
                {
                    fin.AverageVolume = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Shares Short (as of"))
                {
                    fin.CurrentSharesShort = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Shares Short (prior month)"))
                {
                    fin.LastMonthSharesShort = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Forward Annual Dividend Yield"))
                {
                    fin.DividendYieldPercent = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("50-Day Moving Average"))
                {
                    fin.FiftyMovingAverage = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("200-Day Moving Average"))
                {
                    fin.TwoHundredMovingAverage = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("52-Week High"))
                {
                    fin.YearHigh = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("52-Week Low"))
                {
                    fin.YearLow = GetDecimalFromConString(div.NextSibling.ChildNodes[0].InnerHtml);
                }
            }
        }

        public Company GetCompanyProfile(string symbol)
        {
            Dictionary<string, string> profile = new Dictionary<string, string>();
            string url = "http://finance.yahoo.com/q/pr?s=" + symbol + "+Profile";
            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@width='270' and @class='yfnc_modtitlew1']"))
            {
                profile.Add("Name", div.FirstChild.InnerHtml);
            }

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@class='yfnc_tablehead1' and @width='50%']"))
            {
                if (div.InnerHtml.Contains("Sector:"))
                {
                    profile.Add("Sector", div.NextSibling.ChildNodes[0].InnerHtml);
                }
                else if (div.InnerHtml.Contains("Industry:"))
                {
                    profile.Add("Industry", div.NextSibling.ChildNodes[0].InnerHtml);
                }
            }

            return new Company(symbol, profile["Name"], profile["Industry"], profile["Sector"]);
        }

        public decimal GetValueFromInnerHtml(string s)
        {
            if (s.Contains("-"))
            {
                return 0;
            }
            else if (s.Contains("("))
            {
                return Decimal.Parse(s.Trim(), System.Globalization.NumberStyles.AllowParentheses | System.Globalization.NumberStyles.AllowThousands) * 1000; 
            }
            else
            {
                return Convert.ToDecimal(s.Trim().Split('&')[0]) * 1000;
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
            else if (obj.Contains("K"))
            {
                intermediary = obj.Split(new[] { 'K' })[0];
                marketCap = Convert.ToDecimal(intermediary) * 1000;
            }
            else if (obj.Contains("N/A") || obj.Contains("NaN"))
            {
                return 0;
            }
            else if (obj.Contains("%"))
            {
                intermediary = obj.Split(new[] { '%' })[0];
                marketCap = Convert.ToDecimal(intermediary) / 100;
            }
            else
            {
                marketCap = Convert.ToDecimal(obj);
            }

            return marketCap;
        }
    }
}
