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
    public class CompanyBL
    {
        private ICompanyDA _dao; //Data Access object for Company Static Data

        #region CONSTRUCTORS

        private static CompanyBL instance;

        public static CompanyBL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyBL();
                }
                return instance;
            }
        }

        private CompanyBL(ICompanyDA dao)
        { _dao = dao; }

        //Constructor injection - explicitly declares the dependencies of this class
        private CompanyBL()
            : this(new CompanyDA())
        { }

        #endregion

        #region PUBLIC METHODS

        public Company GetCompany(string tickerSymbol)
        { return _dao.GetCompany(tickerSymbol); }

        public CompanyCollection GetCompanies()
        {
            return _dao.GetCompanies();
        }

        #endregion

        #region UPDATE METHODS

        public void UpdateCompany(Company company)
        {
            if (CompanyExists(company.Symbol))
            {
                _dao.UpdateCompany(company);
            }
            else
            {
                _dao.InsertCompany(company);
            }
        }


        #endregion

        #region HELPER METHODS

        internal bool CompanyExists(string tickerSymbol)
        {
            return _dao.CountCompany(tickerSymbol) > 0;
        }

        #endregion
    }
}
