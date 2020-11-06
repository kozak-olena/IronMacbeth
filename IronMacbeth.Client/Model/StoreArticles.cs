using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client
{
    class StoreArticles : Base
    {
        public int Id { get; set; }

        public int StoreId { get; set; }


        public int ArticleId { get; set; }

        public int UserId { get; set; }   //user, who rented book

        private int _productPrice;
        public int ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                Modified = true;
            }
        }
        public new bool Modified { get; set; }
    }
}

