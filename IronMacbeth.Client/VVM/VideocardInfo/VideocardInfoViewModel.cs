using System.Collections.Generic;
using System.Linq;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.VideocardInfo
{
    public class VideocardInfoViewModel : IPageViewModel
    {
        public string PageViewName => "ProcessorInfo";
        public void Update() { }

        public Videocard Videocard { get; }
        public List<StoreSellableItemViewModel> Stores { get; }

        public VideocardInfoViewModel(Videocard videocard)
        {
            Videocard = videocard;

            Stores =
                MainViewModel.ServerAdapter.GetAllStoresSellingVideoCard(videocard.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, videocard, x.StoreVideoCard))
                    .ToList();

        }
    }
}