using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
//using System.ComponentModel;  -- for IBindingList implementation later, see below TODO notes

namespace StockValuationLibrary._2.BOL
{
    public class BaseCollection<T> : ICollection<T>, ICollection, IList<T>, IEnumerable<T>
    {
        protected List<T> _innerList;

        public delegate T AggregateFunction(T best, T candidate);
        
        public BaseCollection()
        {
            _innerList = new List<T>();
        }

        public virtual void Sort(string fieldName, bool descending)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(fieldName);
            Comparison<T> compare = delegate(T a, T b)
            {
                object valueA = propInfo.GetValue(a, null);
                object valueB = propInfo.GetValue(b, null);

                return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
            };
            _innerList.Sort(compare);
            if (descending) _innerList.Reverse();
        }

        public virtual T Aggregate(T Seed, AggregateFunction Func)
        {
            T res = Seed;
            foreach (T t in _innerList)
            { res = Func(res, t); }
            return res;
        }

        public virtual decimal GetSum(string fieldName)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(fieldName);
            decimal tot = 0;
            foreach (T obj in _innerList)
            {
                tot += (decimal)propInfo.GetValue(obj, null);
            }
            return Math.Round(tot, 2);
        }

        public virtual decimal GetAverage(string fieldname)
        {
            return (GetSum(fieldname) / _innerList.Count);
        }

        public virtual decimal GetStandardDeviation(string fieldName)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(fieldName);

            double n = 0;
            double sum = 0;
            double x = 0;
            double avg = Convert.ToDouble(GetAverage(fieldName));

            foreach (T obj in _innerList)
            {
                n += 1;
                x = Convert.ToDouble(propInfo.GetValue(obj, null));
                sum += Math.Pow((double)(x - avg),2); 
            }

            return Convert.ToDecimal(Math.Sqrt(sum / n));
        }

        public virtual decimal GetMax(string fieldName)
        {
            Sort(fieldName, true);
            PropertyInfo propInfo = typeof(T).GetProperty(fieldName);

            return Convert.ToDecimal(propInfo.GetValue(_innerList[0]));
        }

        public virtual decimal GetMin(string fieldName)
        {
            Sort(fieldName, false);
            PropertyInfo propInfo = typeof(T).GetProperty(fieldName);

            return Convert.ToDecimal(propInfo.GetValue(_innerList[0]));
        }

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public void Add(T item)
        {
            _innerList.Add(item);
        }

        public void AddRange(IList<T> items)
        {
            _innerList.AddRange(items);
        }

        public void Clear()
        {
            _innerList.Clear();
        }

        public bool Contains(T item)
        {
            return _innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return _innerList.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        #endregion

        #region IList<T> Members

        public virtual int IndexOf(T item)
        {
            return _innerList.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            _innerList.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }

        public virtual T this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                _innerList[index] = value;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < _innerList.Count; i++)
            {
                array.SetValue(_innerList[i], index);
                index++;
            }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion
    }
}
