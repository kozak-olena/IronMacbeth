using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Model.ToBeRemoved
{
    class StoreBooks : Base<StoreBooks>, IInformationContainer
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public string Date { get; set; }

        public int BookId { get; set; }

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

        public override string DisplayString =>
           $"StoreBook: StoreId: {StoreId} BookId: {BookId}";   //TODO: id of order or rent

        public string InfoContainerKey => "Purchase";
    }
}
