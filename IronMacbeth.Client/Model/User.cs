using IronMacbeth.BFF.Contract;

namespace IronMacbeth.Client
{
    public class User
    {
        public string Login { get; set; }

        public UserRole UserRole { get; set; }

        public bool IsAdmin => UserRole == UserRole.Admin;
    }
}
