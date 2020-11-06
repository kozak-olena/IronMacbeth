using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
    class StoreArticles
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public int ArticleId { get; set; }

        public int UserId { get; set; }

        public int ProductPrice { get; set; }
    }
}

