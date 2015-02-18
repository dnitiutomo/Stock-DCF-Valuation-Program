using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockValuationLibrary._2.BOL
{
    public class XbrlNode
    {
        #region PRIVATE MEMBERS

        private string _NODE_ID;
        private string _PARENT_ID;
        private int _WEIGHT;

        #endregion

        #region PUBLIC PROPERTIES

        public string nodeId
        { get { return _NODE_ID; } set { _NODE_ID = value; } }

        public string parentId
        { get { return _PARENT_ID; } set { _PARENT_ID = value; } }

        public int weight
        { get { return _WEIGHT; } set { _WEIGHT = value; } }

        #endregion

        #region CONSTRUCTORS

        public XbrlNode()
        {
            Initialize();
        }

        public XbrlNode(string nodeid, string parentid, int weight)
        {
            this._NODE_ID = nodeid;
            this._PARENT_ID = parentid;
            this._WEIGHT = weight;
        }

        #endregion

        #region HELPER METHODS

        private void Initialize()
        {
            this._NODE_ID = string.Empty;
            this._PARENT_ID = string.Empty;
            this._WEIGHT = 0;
        }

        #endregion
    }
}
