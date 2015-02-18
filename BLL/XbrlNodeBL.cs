using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.DAL.DA;
using System.Xml;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlNodeBL
    {
        private IXbrlNodeDA _dao;
      
        #region CONSTRUCTORS

        private XbrlNodeBL(IXbrlNodeDA dao)
        { _dao = dao;  }

        public XbrlNodeBL()
            : this(new XbrlNodeDA())
        { }

        #endregion

        #region PUBLIC METHODS

        public XbrlNodeCollection GetXbrlNodes()
        {
            return _dao.GetXbrlNodes();
        }

        public XbrlNodeCollection GetXbrlNodesWithParent(string node)
        {
            return _dao.GetXbrlNodesWithParent(node);
        }

        public void AddXbrlNodes(XbrlDocument xbrlDoc)
        {
            foreach (var frag in xbrlDoc.XbrlFragments)
            {
                foreach (XbrlSchema schema in frag.Schemas)
                {
                    if (schema != null)
                    {
                        foreach (JeffFerguson.Gepsio.LinkbaseDocument linkbase in schema.LinkbaseDocuments)
                        {
                            if (linkbase.CalculationLinks.Count != 0)
                            {
                                foreach (CalculationLink calcLink in linkbase.CalculationLinks)
                                {
                                    if (calcLink.CalculationArcs.Count != 0)
                                    {
                                        foreach (CalculationArc calcArc in calcLink.CalculationArcs)
                                        {
                                            if (!NodeExists(calcArc.ToLocators[0].HrefResourceId.Split(new[] { '_' })[1], calcArc.FromLocator.HrefResourceId.Split(new[] { '_' })[1]))
                                            {
                                                XbrlNode nodeToAdd = new XbrlNode(
                                                    calcArc.ToLocators[0].HrefResourceId.Split(new[] { '_' })[1],
                                                    calcArc.FromLocator.HrefResourceId.Split(new[] { '_' })[1], 
                                                    (int)calcArc.Weight);
                                                InsertXbrlNode(nodeToAdd);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ProcessXbrlInstanceDocument(string xmlUrl, Dictionary<string, List<string>> xbrlTaxonomyTree)
        {
            JeffFerguson.Gepsio.XbrlDocument xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(xmlUrl);

            //AddXbrlNodes(xbrlDoc);

            int yearsToLoad = 3;

            string fileName = System.IO.Path.GetFileNameWithoutExtension(xmlUrl);
            string tickerSymbol = fileName.Split(new[] { '-' })[0];
            int year = GetFiscalYear(tickerSymbol, xbrlDoc.XbrlFragments[0]);

            IncomeStatement incToAdd = new IncomeStatement();
            BalanceSheet balToAdd = new BalanceSheet();
            CompanyAnnualData compAnnToAdd = new CompanyAnnualData();

            Dictionary<string, Object> yahooStats = GetYahooStatistics(tickerSymbol);

            CreateCompanyObject(tickerSymbol, xbrlDoc, yahooStats);

            for (int y = year; y > year - yearsToLoad; y--)
            {
                XbrlIncomeStatementFilter incFilter = new XbrlIncomeStatementFilter();
                XbrlBalanceSheetFilter bsFilter = new XbrlBalanceSheetFilter();
                XbrlCompanyAnnualFilter compAnFilter = new XbrlCompanyAnnualFilter();
                
                incToAdd = incFilter.Populate(this, xbrlDoc, tickerSymbol, y, xbrlTaxonomyTree);
                balToAdd = bsFilter.Populate(xbrlDoc, tickerSymbol, y, xbrlTaxonomyTree);
                compAnnToAdd = compAnFilter.Populate(xbrlDoc, tickerSymbol, y);

                compAnnToAdd.LeveredBeta = Convert.ToDecimal(yahooStats["Beta"]);
                compAnnToAdd.DividendYield = Convert.ToDecimal(yahooStats["DividendYield"]);

                IncomeStatementBL.Instance.UpdateIncomeStatement(incToAdd);
                BalanceSheetBL.Instance.UpdateBalanceSheet(balToAdd);
                CompanyAnnualDataBL.Instance.UpdateCompanyAnnual(compAnnToAdd);
            }
        }

        #endregion

        #region DATABASE METHODS

        public void InsertXbrlNode(XbrlNode node)
        {
            _dao.InsertXbrlNode(node);
        }

        #endregion

        #region HELPER METHODS

        private void CreateCompanyObject(string symbol, JeffFerguson.Gepsio.XbrlDocument xbrlDoc, Dictionary<string, Object> yahooStats)
        {
            XbrlCompanyFilter compFilter = new XbrlCompanyFilter();
            Company compToAdd = new Company();
            compToAdd = compFilter.Populate(xbrlDoc, symbol);

            compToAdd.Industry = Convert.ToString(yahooStats["Industry"]);
            compToAdd.Sector = Convert.ToString(yahooStats["Sector"]);
            CompanyBL.Instance.UpdateCompany(compToAdd);
        }

        public bool NodeExists(string nodeId, string parentId)
        {
            int nodeCount = _dao.CountXbrlNode(nodeId, parentId);
            if (nodeCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsAnnualItem(Item item)
        {
            bool isValid = false;

            if (item.ContextRef.DurationPeriod)
            {
                System.DateTime startDate = item.ContextRef.PeriodStartDate;
                System.DateTime endDate = item.ContextRef.PeriodEndDate;

                if (endDate.Subtract(startDate).Days > 360)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private static int GetFiscalYear(string symbol, XbrlFragment frag)
        {
            int yr = 0;
            foreach (XmlAttribute attrib in frag.XbrlRootNode.Attributes)
            {
                if (attrib.LocalName == symbol)
                {
                    yr = Convert.ToInt32(System.IO.Path.GetFileNameWithoutExtension(attrib.Value).Substring(0, 4));
                }
            }

            return yr;
        }

        #endregion

        #region TREE METHODS

        public Dictionary<string, List<string>> PopulateXbrlTaxonomyTree()
        {
            Dictionary<string, List<string>> xbrlTaxonomyTree = new Dictionary<string, List<string>>();

            XbrlNodeCollection nodeList = GetXbrlNodes();

            foreach (XbrlNode node in nodeList)
            {
                if (!xbrlTaxonomyTree.ContainsKey(node.nodeId))
                {
                    xbrlTaxonomyTree.Add(node.nodeId, GetChildNodes(node.nodeId, nodeList));
                }   
            }

            return xbrlTaxonomyTree;
        }

        public bool HasChild(Dictionary<string, List<string>> xbrlTaxonomyTree, string nodeId)
        {
            if (xbrlTaxonomyTree[nodeId].Count == 0)
            {
                return true;
            }
            return false;
        }

        private List<string> GetChildNodes(string nodeId, XbrlNodeCollection unfilteredNodes)
        {
            List<string> childNodes = new List<string>();

            foreach (XbrlNode node in unfilteredNodes)
            {
                if (node.parentId.Equals(nodeId))
                {
                    childNodes.Add(node.nodeId);
                }
            }

            return childNodes;
        }

        private Dictionary<string, Object> GetYahooStatistics(string symbol)
        {
            YahooApiGateway yahoo = new YahooApiGateway();
            return yahoo.GetCompanyAnnualStats(symbol);
        }

        #endregion
    }
}
