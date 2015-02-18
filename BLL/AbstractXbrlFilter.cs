using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockValuationLibrary._2.BOL;
using StockValuationLibrary._2.BLL;
using JeffFerguson.Gepsio;

namespace StockValuationLibrary._2.BLL
{
    public abstract class AbstractXbrlFilter
    {
       
        internal decimal GetMaxValue(List<Item> items)
        {
            decimal max = -5000000000;
            foreach (Item item in items)
            {
                decimal value;

                if (decimal.TryParse(item.Value, out value))
                {
                    if (value > max)
                    {
                        max = value;
                    }

                }
            }
            return max == -5000000000 ? 0 : max;
        }

        internal decimal GetAvgValue(List<Item> items)
        {
            decimal total = 0;
            int cnt = 0;

            foreach (Item item in items)
            {
                decimal value;
                if (decimal.TryParse(item.Value, out value))
                {
                    total += value;
                    cnt++;
                }
            }

            int count = (cnt == 0) ? 1 : cnt;

            decimal costOfDebt = total / count;

            if (costOfDebt < 1)
            {
                return costOfDebt;
            }
            else
            {
                return costOfDebt / 100;
            }
        }
    }
}
