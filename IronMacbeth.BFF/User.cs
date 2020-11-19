using IronMacbeth.BFF.Contract;

namespace IronMacbeth.BFF
{
    class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PhoneNumber { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public UserRole UserRole { get; set; }
    }
}
