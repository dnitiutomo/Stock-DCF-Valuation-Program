using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using StockValuationLibrary._2.DAL.IDA;
using StockValuationLibrary._2.BOL;

namespace StockValuationLibrary._2.DAL.DA
{
    public class XbrlNodeDA : IXbrlNodeDA
    {
        #region DATA MEMBERS

        private const string SQL_SELECT_XBRL_NODES =
            @"SELECT * FROM xbrl_nodes;";

        private const string SQL_SELECT_COUNT_XBRL_NODE = "SELECT COUNT(*) FROM xbrl_nodes WHERE NODE_ID = @NODE_ID AND PARENT_ID = @PARENT_ID";

        private const string SQL_SELECT_XBRL_NODES_PARENT = "SELECT * FROM xbrl_nodes WHERE PARENT_ID LIKE @PARENT_ID";

        private const string SQL_INSERT_XBRL_NODES =
            @"INSERT INTO xbrl_nodes
            (NODE_ID,
            PARENT_ID,
            WEIGHT)
            VALUES
            (@NODE_ID,
            @PARENT_ID,
            @WEIGHT)";

        #endregion

        #region PUBLIC METHODS

        public XbrlNodeCollection GetXbrlNodes()
        {
            XbrlNodeCollection nodes = null;

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_XBRL_NODES))
            {
                if (rdr.Read())
                {
                    //If there is one result
                    nodes = new XbrlNodeCollection();
                    //Scroll through the results
                    do
                    {
                        nodes.Add(ConvertReaderToXbrlNodeObject(rdr));
                    }
                    while (rdr.Read());
                }
            }

            return nodes;
        }

        public XbrlNodeCollection GetXbrlNodesWithParent(string node)
        {
            XbrlNodeCollection nodes = null;

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@PARENT_ID", MySqlDbType.VarChar)
            };
            parms[0].Value = string.Format("%{0}%", node);

            //Execute Query
            using (MySqlDataReader rdr = MySqlHelper.ExecuteReader(MySqlHelper.SV_CONN_STRING, SQL_SELECT_XBRL_NODES_PARENT, parms))
            {
                if (rdr.Read())
                {
                    //If there is one result
                    nodes = new XbrlNodeCollection();
                    //Scroll through the results
                    do
                    {
                        nodes.Add(ConvertReaderToXbrlNodeObject(rdr));
                    }
                    while (rdr.Read());
                }
            }

            return nodes;
        }

        public int CountXbrlNode(string nodeId, string parentId)
        {
            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@NODE_ID", MySqlDbType.VarChar),
                new MySqlParameter("@PARENT_ID", MySqlDbType.VarChar)
            };
            parms[0].Value = nodeId;
            parms[1].Value = parentId;

            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                try
                {
                    conn.Open();
                    return Convert.ToInt32(MySqlHelper.ExecuteScalar(conn, SQL_SELECT_COUNT_XBRL_NODE, parms));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public void InsertXbrlNode(XbrlNode node)
        {
            MySqlParameter[] parms = GetXbrlNodeParameters();
            SetXbrlNodeParameters(node, parms);
            using (MySqlConnection conn = new MySqlConnection(MySqlHelper.SV_CONN_STRING))
            {
                conn.Open();
                MySqlHelper.ExecuteNonQuery(conn, SQL_INSERT_XBRL_NODES, parms);
            }
        }

        #endregion

        #region HELPER METHODS

        private XbrlNode ConvertReaderToXbrlNodeObject(MySqlDataReader rdr)
        {
            XbrlNode node = new XbrlNode();
            node.nodeId = MySqlHelper.ConvertReaderToString(rdr, "NODE_ID");
            node.parentId = MySqlHelper.ConvertReaderToString(rdr, "PARENT_ID");
            node.weight = MySqlHelper.ConvertReaderToInt(rdr, "WEIGHT");

            return node;
        }

        private static MySqlParameter[] GetXbrlNodeParameters()
        {
            MySqlParameter[] parms;
            parms = new MySqlParameter[] {
                new MySqlParameter("@NODE_ID", MySqlDbType.VarChar),
				new MySqlParameter("@PARENT_ID", MySqlDbType.VarChar),
				new MySqlParameter("@WEIGHT", MySqlDbType.Int32)
            };

            return parms;
        }

        private static void SetXbrlNodeParameters(XbrlNode node, MySqlParameter[] parms)
        {
            parms[0].Value = node.nodeId;
            parms[1].Value = node.parentId;
            parms[2].Value = node.weight;
        }

        #endregion

    }
}