using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.BLL
{
    public class XbrlDocumentScheduler
    {
        private XbrlDocumentLoaderBL xbrlLoader;
        private string[] syms1;
        private string[] syms2;
        private XbrlNodeBL xbrlMngr;

        public XbrlDocumentScheduler(string[] compsList1, string[] compsList2)
        {
            this.xbrlLoader = new XbrlDocumentLoaderBL();
            this.xbrlMngr = new XbrlNodeBL();
            syms1 = compsList1;
            syms2 = compsList2;
        }
        
        public void runThreadOne()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    string xml = xbrlLoader.GetXbrlDocument(syms1[i]);
                    ProcessXbrl(xml);
                    Console.WriteLine("Loaded {0}!", syms1[i]);
                }
                catch
                {
                    Console.WriteLine("FAILED {0}", syms1[i]);
                }
            }
            Console.WriteLine("-----------Finished thread 1-----------");
        }

        public void runThreadTwo()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    string xml = xbrlLoader.GetXbrlDocument(syms2[i]);
                    ProcessXbrl(xml);
                    Console.WriteLine("Loaded {0}!", syms2[i]);
                }
                catch
                {
                    Console.WriteLine("FAILED {0}", syms2[i]);
                }
            }
            Console.WriteLine("-----------Finished thread 2-----------");
        }

        private void ProcessXbrl(string xmlUrl)
        {
            //this.xbrlMngr.ProcessXbrlInstanceDocument(xmlUrl);
        }
    }
}
