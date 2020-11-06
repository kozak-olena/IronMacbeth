using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
    class StoreThesis
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public string Date { get; set; }

        public int ThesisId { get; set; }

        public int UserId { get; set; }

        public int ProductPrice { get; set; }
    }
}

