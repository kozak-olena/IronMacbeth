using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    [Serializable]
    public class Videocard : Base 
    {
        
        public int Id { get; set; }
        
        public int Memory { get; set; }
        
        public int GPUFrequency { get; set; }
        
        public int MemoryFrequency { get; set; }
        
        public int Bus { get; set; }

        
        public string GPU { get; set; }
        
        public string MemoryType { get; set; }
        
        public string Interface { get; set; }
        
        public string Cooling { get; set; }
        
        public string Model { get; set; }
        
        public string MPN { get; set; }
        public string Name => Model;
        public string SellableType => "Videocard";

        #region IDisplayable
        
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