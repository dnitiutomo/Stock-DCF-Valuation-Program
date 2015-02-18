using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;
using System.Xml;
using JeffFerguson.Gepsio;


namespace StockValuationLibrary._2.BLL
{
    public class CompanyAnnualDataBL
    {
        private ICompanyAnnualDataDA _dao; //Data Access object for Company

        #region CONSTRUCTORS

        private static CompanyAnnualDataBL instance;

        public static CompanyAnnualDataBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyAnnualDataBL();
                }
                return instance;
            }
        }

        private CompanyAnnualDataBL(ICompanyAnnualDataDA dao)
        { _dao = dao; }

        //Constructor injection - explicitly declares the dependencies of this class
        private CompanyAnnualDataBL()
            : this(new CompanyAnnualDataDA())
        { }

        #endregion

        #region PUBLIC METHODS

        public CompanyAnnualData GetCompanyAnnual(string tickerSymbol, int year)
        { return _dao.GetCompanyAnnual(tickerSymbol, year); }

        #endregion

        #region UPDATE METHODS

        public void UpdateCompanyAnnual(CompanyAnnualData comp)
        {
            if (CompanyAnnualExists(comp.Symbol, comp.Year))
            {
                _dao.UpdateCompanyAnnual(comp);
            }
            else if (comp.SharesOutstanding != 0)
            {
                _dao.InsertCompanyAnnual(comp);
            }
        }

        #endregion

        #region HELPER METHODS

        internal bool CompanyAnnualExists(string tickerSymbol, int year)
        {
            return _dao.CountCompanyAnnual(tickerSymbol, year) > 0;
        }

        #endregion
    }
}
