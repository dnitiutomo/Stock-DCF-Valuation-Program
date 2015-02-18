using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;

namespace StockValuationLibrary._2.BLL
{
    public class QuandlApiGateway
    {
        private static string QUANDL_COMPANY_URL_PREFIX = "http://www.quandl.com/api/v1/datasets/OFDP/DMDRN_";
        private static string QUANDL_COMPANY_URL_SUFFIX_WITH_KEY = "_ALLFINANCIALRATIOS.csv?auth_token=cGqYUquVzBE4G3RTq3yS";
        private static string QUANDL_COMPANY_URL_SUFFIX_WITHOUT_KEY = "_ALLFINANCIALRATIOS.csv";

        public UsStockCollection ReadQuandlFromStockCsv(string symbol)
        {
            string url = QUANDL_COMPANY_URL_PREFIX + symbol + QUANDL_COMPANY_URL_SUFFIX_WITH_KEY;
            //  string url = QUANDL_COMPANY_URL_PREFIX + symbol + QUANDL_COMPANY_URL_SUFFIX_WITHOUT_KEY;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            var reader = new StreamReader(resp.GetResponseStream());

            Dictionary<string, string> columnMappings = ColumnMappingsDA.Instance.GetColumnMappings();

            UsStockCollection stocks = new UsStockCollection();

            List<string> columnNames = new List<string>();

            List<List<string>> fieldValues = new List<List<string>>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                List<string> yearValues = new List<string>();
                for (int i = 0; i < values.Length; i++)
                {
                    yearValues.Add(values[i]);
                }
                fieldValues.Add(yearValues);
                
            }

            for (int i = 1; i < fieldValues.Count; i++)
            {
                UsStock stock = new UsStock();
                stock.Symbol = symbol;
                for (int j = 0; j < fieldValues[i].Count; j++)
                {
                    if (fieldValues[0][j] == "Date")
                    {
                        PropertyInfo prop = stock.GetType().GetProperty(columnMappings[fieldValues[0][j]], BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(stock, Convert.ToInt16(fieldValues[i][j].Split('-')[0]));
                    }
                    else
                    {
                        PropertyInfo prop = stock.GetType().GetProperty(columnMappings[fieldValues[0][j]], BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(stock, FormatDecimal(fieldValues[i][j]));
                    }
                }

                stocks.Add(stock);
            }

            return stocks;
        }

        public void RefreshAllUsStockData()
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\Daniel\Documents\Trading Documents\QUANDL\stock_list.csv"));
            //var reader = new StreamReader(File.OpenRead(@"C:\Users\Daniel\Documents\Trading Documents\QUANDL\A_COMPANIES_MISSED.csv"));
            bool start = false;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (!line.Contains("Ticker"))
                {
                    string symbol = values[0];

                    if (symbol.Equals("WFD"))
                    {
                        start = true;
                    }
                    if (start)
                    {
                        try
                        {
                            UsStockCollection stocks = ReadQuandlFromStockCsv(symbol);

                            foreach (UsStock stock in stocks)
                            {
                                UsStockBL.Instance.UpdateUsStock(stock);
                            }
                            Console.WriteLine("Successfully Loaded {0}", symbol);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("ERROR: DID NOT LOAD symbol: {0} {1}", symbol, ex.Message));
                        }
                    }
                }
            }
        }

        public void RefreshSingleUsStockData(string symbol)
        {
            UsStockCollection stocks = ReadQuandlFromStockCsv(symbol);
            foreach (UsStock stock in stocks)
            {
                UsStockBL.Instance.UpdateUsStock(stock);
            }
            Console.WriteLine("Successfully Loaded {0}", symbol);
            
        }

        public decimal FormatDecimal(string number)
        {
            if (number.Contains("e") || number.Contains("E"))
            {
                double parsed = Double.Parse(number, System.Globalization.NumberStyles.Float);
                return Convert.ToDecimal(parsed);
            }
            else if (number != null && number != "")
            {
                return Convert.ToDecimal(number);
            }


            return 0;
        }

        public List<string> GetAllCompanies()
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\Daniel\Documents\Trading Documents\QUANDL\quandl-stock-code-list.csv"));
            List<string> stocks = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (!line.Contains("Ticker"))
                {
                    string symbol = values[0];

                    stocks.Add(symbol);
                }
            }
            return stocks;
        }
    }
}
