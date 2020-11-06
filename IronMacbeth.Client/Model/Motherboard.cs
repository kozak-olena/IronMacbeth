using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    [Serializable]
    public class Motherboard : Base, ISellable
    {
        
        public int Id { get; set; }
        
        public int DIMM { get; set; }
        
        public int LAN { get; set; }
        
        public int USB { get; set; }

        
        public string CPUSocket { get; set; }
        
        public string Northbridge { get; set; }
        
        public string Southbridge { get; set; }
        
        public string GraphicalInterface { get; set; }
        
        public string Model { get; set; }
        
        public string MPN { get; set; }
        public string Name => Model;

        public string SellableType => "Motherboard";

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
            //get { return Links.Count; }
            get { return 0; }
        }
    }
}