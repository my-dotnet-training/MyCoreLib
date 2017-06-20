using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.Common.Model
{
    public abstract class BaseClassIndexable<T>
    {
        public abstract T CallByIndex(int index);
        public abstract void SetByIndex(int index, T value);
        public abstract T CallByKey(string key);
        public abstract void SetByKey(string key, T value);

        public T this[int index]
        {
            get
            {
                return CallByIndex(index);
            }
            set
            {
                SetByIndex(index, value);
            }
        }
        public T this[string key]
        {
            get
            {
                return CallByKey(key);
            }
            set
            {
                SetByKey(key, value);
            }
        }
    }
}
