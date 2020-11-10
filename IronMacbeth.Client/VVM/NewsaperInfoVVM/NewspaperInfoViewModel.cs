using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using System.Collections.Generic;
using System.Linq;


namespace IronMacbeth.Client.VVM
{
    public class NewspaperInfoViewModel
    {
        public string PageViewName => "NewspaperInfo";
        public void Update() { }

        public Newspaper Newspaper { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public NewspaperInfoViewModel(Newspaper newspaper)
        {
            Newspaper = newspaper;

            Stores =
                MainViewModel.ServerAdapter.GetAllStoresSellingMemory(newspaper.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, newspaper, x.StoreMemory))          //
                    .ToList();
        }
    }
}
