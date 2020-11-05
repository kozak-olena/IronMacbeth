using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Motherboard : Base<Motherboard>, IInformationContainer, ISellable
    {
        [Database]
        public int Id { get; set; }
        [Database]
        public int DIMM { get; set; }
        [Database]
        public int LAN { get; set; }
        [Database]
        public int USB { get; set; }

        [Database]
        public string CPUSocket { get; set; }
        [Database]
        public string Northbridge { get; set; }
        [Database]
        public string Southbridge { get; set; }
        [Database]
        public string GraphicalInterface { get; set; }
        [Database]
        public string Model { get; set; }
        [Database]
        public string MPN { get; set; }
        public string Name => Model;

        public string SellableType => "Motherboard";

        public string InfoContainerKey => "Motherboard";

        public override string DisplayString =>
            $"Motherboard: Id: {Id}";

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

        //public List<Store> Stores
        //{
        //    get { return MainViewModel.ServerAdapter.GetAllStores().Where(item => item.MotherboardId == Id).Select(item => item.Store).ToList(); }
        //}

        //public List<StoreMotherboard> Links
        //{
        //    get { return MainViewModel.ServerAdapter.GetAllStoreMotherboards().Where(item => item.MotherboardId == Id).ToList(); }
        //}

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