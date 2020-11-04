using System;
using System.Collections.Generic;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public abstract class Base<T>
    {
        public static List<T> Items = new List<T>();

        protected Base()
        {
            Items.Add((T)Convert.ChangeType(this, typeof(T)));
        }

        public bool ToBeRemoved = false;
        public bool Modified = false;
        public bool ToBeAdded = false;

        public abstract string DisplayString { get; }
    }
}
