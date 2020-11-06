using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
    class StoreNewspaper
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        public int ProductPrice { get; set; }
    }
}

