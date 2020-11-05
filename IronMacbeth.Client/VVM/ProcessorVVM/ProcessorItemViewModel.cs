using System.Windows.Media.Imaging;
using IronMacbeth.Client.VVM.ProcessorInfo;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MemoryVVM
{
    public class ProcessorItemViewModel
    {
        private Processor _item;

        public ProcessorItemViewModel(Processor item)
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

        public string Socket => _item.Socket;
        public int Level2Cache => _item.Level2Cache;
        public int Level3Cache => _item.Level3Cache;
        public int NumberOfCores => _item.NumberOfCores;
        public int Lithography => _item.Lithography;

        public ProcessorInfoViewModel MoreInfoVm => new ProcessorInfoViewModel(_item);
    }
}