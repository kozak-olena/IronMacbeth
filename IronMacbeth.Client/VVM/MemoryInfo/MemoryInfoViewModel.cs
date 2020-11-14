﻿using System.Collections.Generic;
using System.Linq;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;

namespace IronMacbeth.Client.VVM.MemoryInfo
{
    public class MemoryInfoViewModel : IPageViewModel
    {
        public string PageViewName => "MemoryInfo";
        public void Update() { }

        public Memory Memory { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public MemoryInfoViewModel(Memory memory)
        {
            Memory = memory;

            Stores =
                ServerAdapter.Instance.GetAllStoresSellingMemory(memory.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, memory, x.StoreMemory))
                    .ToList();
        }
    }
}