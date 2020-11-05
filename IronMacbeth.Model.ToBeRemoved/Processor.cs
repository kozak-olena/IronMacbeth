using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Processor:Base<Processor>,IInformationContainer,ISellable
    {
        [Database]
        public int Id { get; set; }
        [Database]
        public int NumberOfCores { get; set; }
        [Database]
        public int Lithography { get; set; }
        [Database]
        public int TDP { get; set; }
        [Database]
        public int Level2Cache { get; set; }
        [Database]
        public int Level3Cache { get; set; }

        [Database]
        public string BaseFrequency { get; set; }
        [Database]
        public string TurboFrequency { get; set; }
        [Database]
        public string Socket { get; set; }
        [Database]
        public string ProcessorCore { get; set; }
        [Database]
        public string ProcessorGraphics { get; set; }
        [Database]
        public string Model { get; set; }
        [Database]
        public string MPN { get; set; }
        public string Name => Model;
        public string SellableType => "Processor";

        //public string InfoContainerKey => "Processor";

        public override string DisplayString => 
            $"Processor: Id: {Id}";

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
        //    get { return MainViewModel.ServerAdapter.GetAllStoreProcessors().Where(item => item.ProcessorId == Id).Select(item => item.Store).ToList(); }
        //}

        //public List<StoreProcessor> Links
        //{
        //    get { return MainViewModel.ServerAdapter.GetAllStoreProcessors().Where(item => item.ProcessorId == Id).ToList(); }
        //}

        public int AveragePrice
        {
            get
            {
                //var links = Links;
                //if (links.Count != 0)
                //{
                //    return links.Sum(link => link.ProductPrice)/links.Count;
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

        public string InfoContainerKey => "Processor";
    }
}