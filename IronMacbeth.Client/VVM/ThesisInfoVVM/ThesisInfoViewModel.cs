using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.ThesisInfoVVM
{
  public  class ThesisInfoViewModel
    {
        public string PageViewName => "NewspaperInfo";
        public void Update() { }

        public Thesis Thesis { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public ThesisInfoViewModel(Thesis thesis)
        {
            Thesis = thesis;

            Stores =
                MainViewModel.ServerAdapter.GetAllStoresSellingMemory(thesis.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, thesis, x.StoreMemory))          //
                    .ToList();
        }
    }
}
