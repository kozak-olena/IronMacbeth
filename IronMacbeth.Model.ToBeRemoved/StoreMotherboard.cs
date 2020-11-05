using System;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class StoreMotherboard : Base<StoreMotherboard>, IInformationContainer, ISellableLink
    {
            [Database]
            public int Id { get; set; }
            [Database]
            public int StoreId { get; set; }
            [Database]
            public int MotherboardId { get; set; }


            private int _productPrice;

            [Database]
            public int ProductPrice
            {
                get { return _productPrice; }
                set
                {
                    _productPrice = value;
                    Modified = true;
                }
            }

            private int _productWarranty;

            [Database]
            public int ProductWarranty
            {
                get { return _productWarranty; }
                set
                {
                    _productWarranty = value;
                    Modified = true;
                }
            }

            public new bool Modified { get; set; }


            //public Store Store
            //{
            //    get { return MainViewModel.ServerAdapter.GetAllStores().Find(item => item.Id == StoreId); }
            //}

            //public Motherboard Motherboard
            //{
            //    get { return MainViewModel.ServerAdapter.GetAllMotherboards().Find(item => item.Id == MotherboardId); }
            //}

            public override string DisplayString =>
                $"StoreMotherboard: StoreId: {StoreId} VideocardId: {MotherboardId}";

            public string InfoContainerKey => "Purchase";

            #region ISellableLink

            public int SellableId
            {
                get { return MotherboardId; }
                set { MotherboardId = value; }
            }

            #endregion
    }
}