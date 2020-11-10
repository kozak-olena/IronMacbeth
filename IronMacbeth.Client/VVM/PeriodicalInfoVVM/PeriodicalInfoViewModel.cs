using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.PeriodicalInfoVVM
{
    public class PeriodicalInfoViewModel
    {
        public string PageViewName => "PeriodicalInfo";
        public void Update() { }

        public Periodical Periodical { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public PeriodicalInfoViewModel(Periodical periodical)
        {
            Periodical = periodical;

            Stores =
                MainViewModel.ServerAdapter.GetAllStoresSellingMemory(periodical.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, periodical, x.StoreMemory))          //
                    .ToList();
        }
    }
}
