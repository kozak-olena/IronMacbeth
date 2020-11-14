using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF
{
    public static class UserLoginService
    {
        public static bool VerifyUserCredentials(string login, string password)
        {
            User user;

            using (var dbContext = new DbContext())
            {
                user = dbContext.Users.Where(x => x.Login == login).SingleOrDefault();
            }

            if (user == null)
            {
                return false;
            }

            var result = SecurePasswordHasher.Verify(password, user.PasswordHash);

            return result;
        }
    }
}
