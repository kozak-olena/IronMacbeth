using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    [Serializable]
    public class Memory : Base, ISellable
    {
        public int Id { get; set; }

        public int Size { get; set; }

        public int Frequency { get; set; }

        public string Type { get; set; }

        public string Standart { get; set; }

        public string Timings { get; set; }
        
        public string Voltage { get; set; }
        
        public string FormFactor { get; set; }
        
        public string Model { get; set; }
        
        public string MPN { get; set; }
        public string Name => Model;
        public string SellableType => "Memory";

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