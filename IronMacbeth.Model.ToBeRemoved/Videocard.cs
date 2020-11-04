using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Videocard : Base<Videocard>, IInformationContainer, ISellable
    {
        [Database]
        public int Id { get; set; }
        [Database]
        public int Memory { get; set; }
        [Database]
        public int GPUFrequency { get; set; }
        [Database]
        public int MemoryFrequency { get; set; }
        [Database]
        public int Bus { get; set; }

        [Database]
        public string GPU { get; set; }
        [Database]
        public string MemoryType { get; set; }
        [Database]
        public string Interface { get; set; }
        [Database]
        public string Cooling { get; set; }
        [Database]
        public string Model { get; set; }
        [Database]
        public string MPN { get; set; }
        public string Name => Model;
        public string SellableType => "Videocard";

        public string InfoContainerKey => "Videocard";

        public override string DisplayString =>
            $"Videocard: Id: {Id}";

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
            get { return StoreVideocard.Items.Where(item => item.VideocardId == Id).Select(item => item.Store).ToList(); }
        }

        public List<StoreVideocard> Links
        {
            get { return StoreVideocard.Items.Where(item => item.VideocardId == Id).ToList(); }
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