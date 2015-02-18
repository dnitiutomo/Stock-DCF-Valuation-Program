using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.DAL.DA
{
    public abstract class AbstractQueryBuilder
    {
        protected static string _TABLE_NAME;
        protected static string[] _NON_KEY_COLS;
        protected static string[] _KEY_COLS;

        protected string SelectAllSQLStatement()
        {
            return @"SELECT * FROM " + _TABLE_NAME;
        }

        protected string CountEntrySQLStatement()
        {
            string statement = @"SELECT COUNT(*) FROM " + _TABLE_NAME + " WHERE ";

            for (int i = 0; i < _KEY_COLS.Length; i++)
            {
                if (i == _KEY_COLS.Length - 1)
                {
                    statement += (_KEY_COLS[i] + " = @" + _KEY_COLS[i]);
                }
                else
                {
                    statement += (_KEY_COLS[i] + " = @" + _KEY_COLS[i] + " AND ");
                }
            }

            return statement;
        }

        protected string SelectEntryFromSQLStatement()
        {
            string statement = @"SELECT * FROM " + _TABLE_NAME + " WHERE ";

            for (int i = 0; i < _KEY_COLS.Length; i++)
            {
                if (i == _KEY_COLS.Length - 1)
                {
                    statement += (_KEY_COLS[i] + " = @" + _KEY_COLS[i]);
                }
                else
                {
                    statement += (_KEY_COLS[i] + " = @" + _KEY_COLS[i] + " AND ");
                }
            }

            return statement;
        }

        protected string InsertSQLStatement()
        {
            string statement = @"INSERT INTO " + _TABLE_NAME + " (";

            foreach (string col in _KEY_COLS)
            {
                statement += (col + ", ");
            }

            for (int i = 0; i < _NON_KEY_COLS.Length; i++)
            {
                if (i == _NON_KEY_COLS.Length - 1)
                {
                    statement += (_NON_KEY_COLS[i] + ") VALUES (");
                }
                else
                {
                    statement += (_NON_KEY_COLS[i] + ", ");
                }
            }

            foreach (string col in _KEY_COLS)
            {
                statement += ("@" + col + ", ");
            }

            for (int i = 0; i < _NON_KEY_COLS.Length; i++)
            {
                if (i == _NON_KEY_COLS.Length - 1)
                {
                    statement += ("@" + _NON_KEY_COLS[i] + ")");
                }
                else
                {
                    statement += ("@" + _NON_KEY_COLS[i] + ", ");
                }
            }

            return statement;
        }

        protected string UpdateSQLStatement()
        {
            string statement = @"UPDATE " + _TABLE_NAME + " SET ";

            for (int i = 0; i < _NON_KEY_COLS.Length; i++)
            {
                if (i == _NON_KEY_COLS.Length - 1)
                {
                    statement += (_NON_KEY_COLS[i] + " = @" + _NON_KEY_COLS[i] + " WHERE ");
                }
                else
                {
                    statement += (_NON_KEY_COLS[i] + " = @" + _NON_KEY_COLS[i] + ", ");
                }
            }

            for (int i = 0; i < _KEY_COLS.Length; i++)
            {
                if (i == _KEY_COLS.Length - 1)
                {
                    statement += (_KEY_COLS[i] + " = @" + _KEY_COLS[i]);
                }
                else
                {
                    statement += (_KEY_COLS[i] + " = @" + _KEY_COLS[i] + " AND ");
                }
            }

            return statement;
        }
    }
}
