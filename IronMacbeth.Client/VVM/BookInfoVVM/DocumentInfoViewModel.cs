using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using System.Collections.Generic;
using System.Linq;

namespace IronMacbeth.Client.VVM.BookVVM
{
    class DocumentInfoViewModel : IPageViewModel
    {
        public string PageViewName => "BookInfo";
        public void Update() { }

        public Book Book { get; }

       // public List<StoreSellableItemViewModel> Stores { get; }

        public DocumentInfoViewModel(Book book)
        {
            Book = book;

             
        }
    }
}
