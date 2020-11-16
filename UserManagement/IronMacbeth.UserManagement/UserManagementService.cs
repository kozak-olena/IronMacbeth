using IronMacbeth.UserManagement.Contract;
using System;

namespace IronMacbeth.UserManagement
{
    class UserManagementService : IUserManagementService
    {
        public DateTime GetCurrentTime()
        {
            Console.WriteLine("Someone asked for the time.");

            return DateTime.UtcNow;
        }
    }
}
