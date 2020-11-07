﻿using System;

namespace IronMacbeth.Client
{
    [Serializable]
    public class StoreMemory : Base, ISellableLink
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