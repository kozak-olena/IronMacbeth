using System;

namespace IronMacbeth.Client
{
    [Serializable]
    public abstract class Base
    {
        public bool ToBeRemoved = false;
        public bool Modified = false;
        public bool ToBeAdded = false;
    }
}
