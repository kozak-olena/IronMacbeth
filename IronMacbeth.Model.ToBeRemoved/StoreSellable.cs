namespace IronMacbeth.Model.ToBeRemoved
{
    public class StoreSellable:Base<StoreSellable>,ISellableLink
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int SellableId { get; set; }
        public ISellable Sellable { get; set; }
        public int ProductPrice { get; set; }
        public int ProductWarranty { get; set; }
        public new bool Modified { get; set; }

        public override string DisplayString => "StoreSellable";
    }
}