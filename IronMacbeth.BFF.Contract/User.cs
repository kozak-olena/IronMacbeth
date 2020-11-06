using System;

namespace IronMacbeth.BFF.Contract
{
    public class User
    {        
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        public int AccessLevel { get; set; }
    }
}
