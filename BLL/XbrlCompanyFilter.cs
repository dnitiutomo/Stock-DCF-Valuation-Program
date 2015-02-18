using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlCompanyFilter
    {
        public Company Populate(XbrlDocument xbrlDoc, string symbol)
        {
            Company comp = new Company();
            comp.Symbol = symbol;
            foreach (XbrlFragment frag in xbrlDoc.XbrlFragments)
            {
                foreach (Item xbrlItem in frag.Facts)
                {
                    comp = CheckItem(xbrlItem, comp);
                }
            }

            return comp;
        }

        private Company CheckItem(Item xbrlItem, Company comp)
        {
            CheckName(xbrlItem, ref comp);

            return comp;
        }

        private void CheckName(Item xbrlItem, ref Company comp)
        {
            if (xbrlItem.Name.Contains("EntityRegistrantName"))
            {
                comp.CompanyName = xbrlItem.Value;
            }
        }

        private void CheckFilerCategory(Item xbrlItem, ref Company comp)
        {
            if (xbrlItem.Name.Contains("EntityFilerCategory"))
            {
                comp.Industry = xbrlItem.Value;
            }
        }
    }

}
