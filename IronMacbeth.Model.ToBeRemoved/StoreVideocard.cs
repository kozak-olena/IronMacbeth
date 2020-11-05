using System;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class StoreVideocard : Base<StoreVideocard>, ISellableLink
    {
        [Database]
        public int Id { get; set; }
        [Database]
        public int StoreId { get; set; }
        [Database]
        public int VideocardId { get; set; }


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

        public override string DisplayString =>
            $"StoreVideocard: StoreId: {StoreId} VideocardId: {VideocardId}";

        public string InfoContainerKey => "Purchase";

        #region ISellableLink

        public int SellableId
        {
            get { return VideocardId; }
            set { VideocardId = value; }
        }

        #endregion
    }
}