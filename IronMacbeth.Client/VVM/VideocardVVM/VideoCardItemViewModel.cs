using System.Windows.Media.Imaging;
using IronMacbeth.Client.VVM.VideocardInfo;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MemoryVVM
{
    public class VideoCardItemViewModel
    {
        private Videocard _item;

        public VideoCardItemViewModel(Videocard item)
        {
            _item = item;
        }

        public BitmapImage BitmapImage => _item.BitmapImage;
        public int AveragePrice => _item.AveragePrice;
        public int MinPrice => _item.MinPrice;
        public int MaxPrice => _item.MaxPrice;
        public int NumberOfOfferings => _item.NumberOfOfferings;
        public string Model => _item.Model;
        public string MPN => _item.MPN;

        public string Name => _item.Name;
        public int GPUFrequency => _item.GPUFrequency;
        public string MemoryType => _item.MemoryType;
        public int Memory => _item.Memory;
        public int MemoryFrequency => _item.MemoryFrequency;
        public int Bus => _item.Bus;
        public string Interface => _item.Interface;

        public VideocardInfoViewModel MoreInfoVm => new VideocardInfoViewModel(_item);
    }
}