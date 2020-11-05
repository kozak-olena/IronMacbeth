using System.Windows.Media.Imaging;
using IronMacbeth.Client.VVM.MemoryInfo;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MemoryVVM
{
    public class MemoryItemViewModel
    {
        private Memory _item;

        public MemoryItemViewModel(Memory item)
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
        public string Standart => _item.Standart;
        public string Timings => _item.Timings;
        public string Voltage => _item.Voltage;

        public MemoryInfoViewModel MoreInfoVm => new MemoryInfoViewModel(_item);
    }
}