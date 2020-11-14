using System.Collections.Generic;
using System.Linq;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;

namespace IronMacbeth.Client.VVM.ProcessorInfo
{
    public class ProcessorInfoViewModel:IPageViewModel
    {
        public string PageViewName => "ProcessorInfo";
        public void Update() { }

        public Processor Processor { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public ProcessorInfoViewModel(Processor processor)
        {
            Processor = processor;

            Stores = 
                ServerAdapter.Instance.GetAllStoresSellingProcessor(processor.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, processor, x.StoreProcessor))
                    .ToList();
        }
    }
}