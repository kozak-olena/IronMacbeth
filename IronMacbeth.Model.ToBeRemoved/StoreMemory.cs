using System;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class StoreMemory : Base<StoreMemory>, IInformationContainer, ISellableLink
    {
         
        public int Id { get; set; }
         
        public int StoreId { get; set; }
       
        public int MemoryId { get; set; }

        private int _productPrice;

        
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

        //public Memory Memory
        //{
        //    get { return MainViewModel.ServerAdapter.GetAllMemories().Find(item => item.Id == MemoryId); }
        //}

        public override string DisplayString =>
            $"StoreMemory: StoreId: {StoreId} MemoryId: {MemoryId}";

        public string InfoContainerKey => "Purchase";

        #region ISellableLink

        public int SellableId
        {
            get { return MemoryId; }
            set { MemoryId = value; }
        }

        #endregion
    }
}