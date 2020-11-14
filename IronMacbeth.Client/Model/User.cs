using System;
using System.Collections.Generic;
using System.Linq;

namespace IronMacbeth.Client
{
    [Serializable]
    public class User:Base
    {
        
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        public int AccessLevel { get; set; }

        public bool IsAdmin => AccessLevel == 9;
    }
}
