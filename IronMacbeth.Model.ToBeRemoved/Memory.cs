using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Memory : Base<Memory>, IInformationContainer, ISellable
    {
        [Database]
        public int Id { get; set; }
        [Database]
        public int Size { get; set; }
        [Database]
        public int Frequency { get; set; }

        [Database]
        public string Type { get; set; }
        [Database]
        public string Standart { get; set; }
        [Database]
        public string Timings { get; set; }
        [Database]
        public string Voltage { get; set; }
        [Database]
        public string FormFactor { get; set; }
        [Database]
        public string Model { get; set; }
        [Database]
        public string MPN { get; set; }
        public string Name => Model;
        public string SellableType => "Memory";

        public string InfoContainerKey => "Memory";

        public override string DisplayString =>
            $"Memory: Id: {Id}";

        #region IDisplayable
        [Database]
        public string ImageName { get; set; }
        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { _bitmapImage = value; }
        }

        [NonSerialized]
        private BitmapImage _bitmapImage;
        #endregion

        #region IDescribable
        [Database]
        public string DescriptionName { get; set; }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [NonSerialized]
        private string _description;
        #endregion

        public List<Store> Stores
        {
            get { return StoreMemory.Items.Where(item => item.MemoryId == Id).Select(item => item.Store).ToList(); }
        }

        public List<StoreMemory> Links
        {
            get { return StoreMemory.Items.Where(item => item.MemoryId == Id).ToList(); }
        }

        public int AveragePrice
        {
            get
            {
                var links = Links;
                if (links.Count != 0)
                {
                    return links.Sum(link => link.ProductPrice) / links.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int MinPrice
        {
            get
            {
                if (Links.Count != 0)
                {
                    return Links.Min(link => link.ProductPrice);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int MaxPrice
        {
            get
            {
                if (Links.Count != 0)
                {
                    return Links.Max(link => link.ProductPrice);
                }
                else
                {
                    return 0;
                }
            }
        }

        public int NumberOfOfferings
        {
            get { return Links.Count; }
        }
    }
}