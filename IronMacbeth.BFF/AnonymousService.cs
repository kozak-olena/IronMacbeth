using IronMacbeth.BFF.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IronMacbeth.BFF
{
    class AnonymousService : IAnonymousService
    {
        public UserRegistrationStatus Register(string login, string password, string surname, string name, int phoneNumber)
        {
            var passwordHash = SecurePasswordHasher.Hash(password);

            using (var dbContext = new DbContext())
            {
                dbContext.Users.Add(new User { Login = login, PasswordHash = passwordHash, Name = name, Surname = surname, PhoneNumber = phoneNumber, UserRole = UserRole.User });

                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Number == 2627) // 2627 = unique (primary key) constraint violation
                {
                    return UserRegistrationStatus.UserNameAlreadyTaken;
                }
            }

            return UserRegistrationStatus.Success;
        }

        public bool Ping()
        {
            return true;
        }
    }
}
