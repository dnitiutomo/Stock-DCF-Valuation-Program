using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.IDA
{
    public interface IXbrlNodeDA
    {
        XbrlNodeCollection GetXbrlNodes();
        void InsertXbrlNode(XbrlNode node);
        int CountXbrlNode(string nodeId, string parentId);
        XbrlNodeCollection GetXbrlNodesWithParent(string node);
    }
}
