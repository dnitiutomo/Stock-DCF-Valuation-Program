using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockValuationLibrary._2.BLL
{
    public class Statistics
    {
        public static decimal StandardDeviation(List<decimal> list)
        {
            double n = 0;
            double sum = 0;
            double x = 0;
            double avg = Convert.ToDouble(Average(list));

            foreach (decimal number in list)
            {
                n += 1;
                x = Convert.ToDouble(number);
                sum += Math.Pow((double)(x - avg), 2);
            }

            return Convert.ToDecimal(Math.Sqrt(sum / n));
        }

        public static decimal Average(List<decimal> list)
        {
            return Sum(list) / list.Count;
        }

        public static decimal Sum(List<decimal> list)
        {
            decimal total = 0;
            foreach (decimal number in list)
            {
                total += number;
            }

            return total;
        }

        public static decimal Max(List<decimal> list)
        {
            return list.Max();
        }

        public static decimal Min(List<decimal> list)
        {
            return list.Min();
        }

        public static decimal SharpRatio(List<decimal> list)
        {
            return Average(list) / StandardDeviation(list);
        }
    }
}
