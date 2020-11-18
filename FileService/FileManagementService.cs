
using IronMacbeth.FileManagement.Contract;
using IronMacbeth.UserManagement.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.FileManagement
{
    class FileManagementService : IFileManagement
    {
        public int Calculate(int i, int a)
        {
            return i + a;
        }
    }
}
