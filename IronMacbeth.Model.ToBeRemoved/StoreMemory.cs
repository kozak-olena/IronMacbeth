using System;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class StoreMemory : Base<StoreMemory>, IInformationContainer, ISellableLink
    {
        [Database]
        public int Id { get; set; }
        [Database]
        public int StoreId { get; set; }
        [Database]
        public int MemoryId { get; set; }


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


        public Store Store
        {
            get { return Store.Items.Find(item => item.Id == StoreId); }
        }

        public Memory Memory
        {
            get { return Memory.Items.Find(item => item.Id == MemoryId); }
        }

        public override string DisplayString =>
            $"StoreMemory: StoreId: {StoreId} MemoryId: {MemoryId}";

        public string InfoContainerKey => "Purchase";

        #region ISellableLink

        public int SellableId
        {
            get { return MemoryId; }
            set { MemoryId = value; }
        }
        public ISellable Sellable => Memory;

        #endregion
    }
}