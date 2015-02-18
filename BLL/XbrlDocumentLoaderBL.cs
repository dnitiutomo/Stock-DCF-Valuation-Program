using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlDocumentLoaderBL
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

            XbrlNodeBL xbrlMngr = new XbrlNodeBL();

            foreach (string symbol in companies)
            {
                try
                {
                    string xmlUrl = GetXbrlDocument(symbol);
                    //xbrlMngr.ProcessXbrlInstanceDocument(xmlUrl);
                    Console.WriteLine("loaded {0}", symbol);
                }
                catch(Exception ex)
                { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", symbol, ex.Message)); }
            }
        }

        public void RefetchAllCompanies()
        {
            CompanyCollection companies = CompanyBL.Instance.GetCompanies();

            XbrlNodeBL xbrlMngr = new XbrlNodeBL();
            Dictionary<string, List<string>> xbrlTaxonomyTree = new Dictionary<string, List<string>>();
            //xbrlTaxonomyTree = xbrlMngr.PopulateXbrlTaxonomyTree();

            bool start = true;

            foreach (Company comp in companies)
            {
                //if (comp.Symbol.Equals("twc"))
                //{
                //    start = true;
                //}

                if (start)
                {
                    try
                    {
                        string xmlUrl = GetXbrlDocument(comp.Symbol);
                        xbrlMngr.ProcessXbrlInstanceDocument(xmlUrl, xbrlTaxonomyTree);
                        Console.WriteLine("loaded {0}", comp.Symbol);
                    }
                    catch (Exception ex)
                    { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", comp.Symbol, ex.Message)); }
                }

            }
        }

        public void GetNyseComponents()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                string url = "http://en.wikipedia.org/wiki/Companies_listed_on_the_New_York_Stock_Exchange_("+ c +")";
                HtmlDocument doc = new HtmlWeb().Load(url);
                List<string> companies = new List<string>();

                foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//a[@href and @rel='nofollow' and @class='external text']"))
                {
                    if (div.Attributes["href"].Value.Contains("ticker=") && div.ParentNode.NextSibling.NextSibling.LastChild.InnerHtml.Equals("USA"))
                    {
                        if (div.InnerHtml.Length < 5)
                        {
                            companies.Add(div.InnerHtml);
                        }
                    }
                }

                XbrlNodeBL xbrlMngr = new XbrlNodeBL();
                Dictionary<string, List<string>> xbrlTaxonomyTree = new Dictionary<string, List<string>>();
                xbrlTaxonomyTree = xbrlMngr.PopulateXbrlTaxonomyTree();
                //bool start = false;
                bool start = true;

                foreach (string symbol in companies)
                {
                    //if (symbol.Equals("XPO"))
                    //{
                    //    start = true;
                    //}
                    if (start)
                    {
                        try
                        {
                            string xmlUrl = GetXbrlDocument(symbol);
                            xbrlMngr.ProcessXbrlInstanceDocument(xmlUrl, xbrlTaxonomyTree);
                            Console.WriteLine("loaded {0}", symbol);
                        }
                        catch (Exception ex)
                        { Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", symbol, ex.Message)); }
                    }
                }
            }
        }

        public string GetXbrlDocument(string symbol)
        {
            string endUrlToSearch = "&filenum=&State=&Country=&SIC=&owner=exclude&Find=Find+Companies&action=getcompany";

            string urlToSearch = "http://www.sec.gov/cgi-bin/browse-edgar?company=&match=&CIK=" + symbol + endUrlToSearch;

            string url = SearchSecForXbrlDoc(urlToSearch);

            string fileName = "http://www.sec.gov";

            HtmlDocument doc = new HtmlWeb().Load(url);

            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//td[@scope='row']"))
            {
                if (div.InnerHtml.Equals("EX-101.INS") || div.InnerHtml.Contains("INSTANCE DOCUMENT"))
                {
                    if (div.NextSibling.NextSibling.FirstChild.Attributes["href"].Value.Contains("xml"))
                    {
                        fileName += div.NextSibling.NextSibling.FirstChild.Attributes["href"].Value;
                        break;
                    }
                }
            }

            if(fileName.Equals("http://www.sec.gov"))
            {
                throw new Exception("XBRL Instance Document Not Found");
            }

            return fileName;
        }

        private static string CheckForXbrlDoc(string urlToSearch, HtmlDocument doc)
        {
            string url = "http://www.sec.gov";
            foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//a[@href and @id='documentsbutton']"))
            {
                if (div.ParentNode.PreviousSibling.PreviousSibling.InnerHtml.Equals("10-K"))
                {
                    url += div.Attributes["href"].Value;
                    break;
                }
            }

            return url;
        }

        private static string SearchSecForXbrlDoc(string BeginUrl)
        {
            string url = BeginUrl;
            bool isDone = false;
            HtmlDocument doc = new HtmlWeb().Load(url);

            while (!isDone)
            {
                string urlToCheck = CheckForXbrlDoc(url, doc);

                if (!urlToCheck.Equals("http://www.sec.gov"))
                {
                    url = urlToCheck;
                    isDone = true;
                }
                else
                {
                    foreach (HtmlNode div in doc.DocumentNode.SelectNodes("//input[@type='button' and @value='Next 40']"))
                    {
                        url = "http://www.sec.gov";
                        string[] toParse = div.Attributes["onClick"].Value.Split('\'');
                        url += toParse[1];
                        doc = new HtmlWeb().Load(url);
                    }
                }
            }
            return url;
        }
    }
}
