using System.Collections.Generic;
using System.Linq;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;

namespace IronMacbeth.Client.VVM.MotherboardInfo
{
    public class MotherboardInfoViewModel : IPageViewModel
    {
        public string PageViewName => "MotherboardInfo";
        public void Update() { }

        public Motherboard Motherboard { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public MotherboardInfoViewModel(Motherboard motherboard)
        {
            Motherboard = motherboard;

            Stores =
                ServerAdapter.Instance.GetAllStoresSellingMotherboard(motherboard.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, motherboard, x.StoreMotherboard))
                    .ToList();
        }
    }
}