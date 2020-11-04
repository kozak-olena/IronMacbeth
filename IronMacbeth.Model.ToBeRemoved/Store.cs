using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Store : Base<Store>, IDisplayable
    {
        [Database]
        public int Id { get; set; }

        [Database]
        public string Name { get; set; }
        [Database]
        public string Delivery { get; set; }
        [Database]
        public string OwnerId { get; set; }
        public override string DisplayString => $"Store: Id: {Id}";

        public List<ISellable> Sellables
        {
            get
            {
                List<ISellable> sellables = new List<ISellable>();

                sellables.AddRange(StoreProcessor.Items.
                        Where(item => item.StoreId == Id).
                        Select(item => item.Processor).ToList());

                sellables.AddRange(StoreVideocard.Items.
                        Where(item => item.StoreId == Id).
                        Select(item => item.Videocard).ToList());

                sellables.AddRange(StoreMotherboard.Items.
                        Where(item => item.MotherboardId == Id).
                        Select(item => item.Motherboard).ToList());

                sellables.AddRange(StoreMemory.Items.
                        Where(item => item.MemoryId == Id).
                        Select(item => item.Memory).ToList());

                return sellables;
            }
        } 

        public List<ISellableLink> SellableLinks
        {
            get
            {
                List<ISellableLink> sellableLinks = new List<ISellableLink>();

                sellableLinks.AddRange(StoreProcessor.Items.Where(item => item.StoreId == Id));
                sellableLinks.AddRange(StoreVideocard.Items.Where(item => item.StoreId == Id));
                sellableLinks.AddRange(StoreMotherboard.Items.Where(item => item.StoreId == Id));
                sellableLinks.AddRange(StoreMemory.Items.Where(item => item.StoreId == Id));

                return sellableLinks;
            }
        }

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

        //public string WarrantyForProduct(object product)
        //{
        //    ISellable sellable = product as ISellable;

        //    object link = GetLink(sellable);
        //    if (link == null)
        //    {
        //        throw new Exception("Link doesn't Exist");
        //    }

        //    var linkType = link.GetType();

        //    int warranty = (int)linkType.GetProperty("ProductWarranty").GetValue(link);

        //    return warranty.ToString();
        //}

        //public int PriceForProduct(object product)
        //{
        //    ISellable sellable = product as ISellable;

        //    object link = GetLink(sellable);
        //    if (link == null)
        //    {
        //        throw new Exception("Link doesn't Exist");
        //    }

        //    var linkType = link.GetType();

        //    int price = (int)linkType.GetProperty("ProductPrice").GetValue(link);

        //    return price;
        //}

        //private object GetLink(ISellable sellable)
        //{
        //    Type type = sellable.GetType();

        //    if (!(type.BaseType == typeof(Base<>).MakeGenericType(type)))
        //    {
        //        throw new Exception("Product doesn't inherit Base<> class");
        //    }

        //    string linkName = "Model.Store" + type.Name;

        //    Assembly assembly = Assembly.GetAssembly(GetType());
        //    Type linkType = assembly.GetType(linkName);

        //    var linkList = linkType.BaseType.GetField("Items").GetValue(sellable);

        //    if (linkList is IEnumerable)
        //    {
        //        int linkStoreId;
        //        int linkSellableId;

        //        foreach (object link in linkList as IEnumerable)
        //        {
        //            linkStoreId = (int)linkType.GetProperty("StoreId").GetValue(link);
        //            linkSellableId = (int)linkType.GetProperty(type.Name + "Id").GetValue(link);

        //            if (linkStoreId == Id && linkSellableId == sellable.Id)
        //            {
        //                return link;
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
