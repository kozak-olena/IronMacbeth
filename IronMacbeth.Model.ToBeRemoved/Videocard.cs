using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Videocard : Base<Videocard>, ISellable
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

        public int AveragePrice
        {
            get
            {
                //TODO: uncomment and fix
                //var links = Links;
                //if (links.Count != 0)
                //{
                //    return links.Sum(link => link.ProductPrice) / links.Count;
                //}
                //else
                //{
                    return 0;
                //}
            }
        }

        public int MinPrice
        {
            get
            {
                //TODO: uncomment and fix
                //if (Links.Count != 0)
                //{
                //    return Links.Min(link => link.ProductPrice);
                //}
                //else
                //{
                return 0;
                //}
            }
        }
        public int MaxPrice
        {
            get
            {
                //TODO: uncomment and fix
                //if (Links.Count != 0)
                //{
                //    return Links.Max(link => link.ProductPrice);
                //}
                //else
                //{
                return 0;
                //}
            }
        }

        public int NumberOfOfferings
        {
            //TODO: uncomment and fix
            //get { return Links.Count; }
            get { return 0; }
        }
    }
}