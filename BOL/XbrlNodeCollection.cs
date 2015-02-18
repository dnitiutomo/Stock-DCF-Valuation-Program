using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class XbrlNodeCollection : BaseCollection<XbrlNode>
    {
        public XbrlNode Find(string nodeId)
        {
            return (_innerList.Find(delegate(XbrlNode e)
            { return (e.nodeId == nodeId); }));
        }
    }
}
