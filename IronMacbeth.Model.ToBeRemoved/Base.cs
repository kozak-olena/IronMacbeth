using System;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public abstract class Base<T>
    {
        public bool ToBeRemoved = false;
        public bool Modified = false;
        public bool ToBeAdded = false;

        public abstract string DisplayString { get; }
    }
}
