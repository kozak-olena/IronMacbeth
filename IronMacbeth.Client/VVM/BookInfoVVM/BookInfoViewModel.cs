using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using IronMacbeth.Model.ToBeRemoved;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.BookVVM
{
    class BookInfoViewModel : IPageViewModel
    {
        public string PageViewName => "BookInfo";
        public void Update() { }

        public Book Book { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public BookInfoViewModel(Book book)
        {
            Book = book;

            Stores =
                MainViewModel.ServerAdapter.GetAllStoresSellingMemory(book.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, book, x.StoreMemory))
                    .ToList();
        }
    }
}
