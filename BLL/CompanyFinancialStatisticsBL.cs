using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;

namespace StockValuationLibrary._2.BLL
{
    public class CompanyFinancialStatisticsBL
    {
        private ICompanyFinancialStatisticsDA _dao;
        private static CompanyFinancialStatisticsBL instance;

        public static CompanyFinancialStatisticsBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyFinancialStatisticsBL();
                }
                return instance;
            }
        }

        private CompanyFinancialStatisticsBL(ICompanyFinancialStatisticsDA dao)
        { _dao = dao; }

        public CompanyFinancialStatisticsBL()
            : this(new CompanyFinancialStatisticsDA())
        { }

        #region PUBLIC METHODS

        public CompanyFinancialStatistics GetCompanyFinancialStatistics(string tickerSymbol, DateTime date)
        { return _dao.GetCompanyFinancialStatistics(tickerSymbol, date); }

        #endregion

        #region UPDATE METHODS

        public void UpdateCompanyFinancialStatistics(CompanyFinancialStatistics cfs)
        {
            if (CompanyFinancialStatisticsExists(cfs.Symbol, cfs.Date))
            {
                _dao.UpdateCompanyFinancialStatistics(cfs);
            }
            else
            {
                _dao.InsertCompanyFinancialStatistics(cfs);
            }
            
        }

        public bool CompanyFinancialStatisticsExists(string symbol, DateTime date)
        {
            if(_dao.CountCompanyFinancialStatistics(symbol, date) > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        public void EnterAllCompaniesFinancialStatistics()
        {
            CompanyCollection companies = CompanyBL.Instance.GetCompanies();
            foreach (Company comp in companies)
            {
                try
                {
                    CompanyFinancialStatistics cfs = new CompanyFinancialStatistics(comp.Symbol, DateTime.Today);
                    YahooHtmlParser.Instance.GetCompaniesFinancialData(cfs);

                    CompanyFinancialStatisticsBL cfsBL = new CompanyFinancialStatisticsBL();

                    cfsBL.UpdateCompanyFinancialStatistics(cfs);

                    Console.WriteLine("LOADED  " + comp.Symbol);
                }
                catch (Exception e)
                {
                    Console.WriteLine("DID NOT LOAD  " + comp.Symbol);
                }
            }
        }

        public void PopulateCompanyFinancialStatisticsFromStatements(CompanyFinancialStatistics finStats, IncomeStatement inc, BalanceSheet bs, CompanyAnnualData cad)
        {
            finStats.BookValuePerShare = bs.ShareholdersEquity / cad.SharesOutstanding;
            finStats.DebtToEquity = bs.Debt / bs.ShareholdersEquity;
            finStats.ProfitMargin = inc.NetIncome / inc.Revenue;
            finStats.ReturnOnAssets = inc.NetIncome / bs.TotalAssets;

            if (bs.ShareholdersEquity == 0)
            {
                finStats.ReturnOnEquity = 0;
            }
            else
            {
                finStats.ReturnOnEquity = inc.NetIncome / bs.ShareholdersEquity;
            }
            
            finStats.RevenuePerShare = inc.Revenue / cad.SharesOutstanding;
            finStats.TotalDebt = bs.Debt;
        }
    }
}
