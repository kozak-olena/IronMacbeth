using IronMacbeth.Client.VVM.BookVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client.VVM.MyOrdersVVM.MyOrdersItemsVVM
{
    class OrderBookItemViewModel : IDocumentViewModel
    {
        private Order _item;

        public OrderBookItemViewModel(Order item)
        {
            _item = item;
        }

        public string Name => GetName(_item.GetOrderedItem());

        public string Author => GetAuthor(_item.GetOrderedItem());

        public int Id => _item.Id;

        public string StatusOfOrder => _item.StatusOfOrder;

        public DateTime DateOfOrder => _item.DateOfOrder;

        public DateTime DateOfReturn => _item.DateOfReturn;

        public DateTime ReceiveDate => _item.ReceiveDate;

        public string UserLogin => _item.UserLogin;

        public string TypeOfDocument => _item.TypeOfOrder;


        public DocumentInfoViewModel MoreInfoVm => new DocumentInfoViewModel(_item.GetOrderedItem());

        public object GetItem()
        {
            return _item;
        }

        public string GetAuthor(object order)
        {
            if (order is Book)
            {
                Book book = (Book)order;
                return book.Author;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public string GetName(object order)
        {
            if (order is Book)
            {
                Book book = (Book)order;
                return book.Name;
            }
            else if (order is Article)
            {
                Article article = (Article)order;
                return article.Name;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }

}
