using System.Windows.Media.Imaging;
using IronMacbeth.Client.VVM.MotherboardInfo;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MemoryVVM
{
    public class MotherboardItemViewModel
    {
        private Motherboard _item;

        public MotherboardItemViewModel(Motherboard item)
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

        public string CPUSocket => _item.CPUSocket;
        public string Northbridge => _item.Northbridge;
        public string Southbridge => _item.Southbridge;
        public string GraphicalInterface => _item.GraphicalInterface;
        public int DIMM => _item.DIMM;
        public int LAN => _item.LAN;
        public int USB => _item.USB;

        public MotherboardInfoViewModel MoreInfoVm => new MotherboardInfoViewModel(_item);
    }
}