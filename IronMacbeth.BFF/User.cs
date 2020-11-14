using IronMacbeth.BFF.Contract;

namespace IronMacbeth.BFF
{
    class User
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public UserRole UserRole { get; set; }
    }
}
