namespace IronMacbeth.Model.ToBeRemoved
{
    public interface ISellableLink
    {
        int Id { get; set; }

        int StoreId { get; set; }
        Store Store { get; }

        int SellableId { get; set; }
        ISellable Sellable { get; }

        int ProductPrice { get; set; }
        int ProductWarranty { get; set; }

        bool Modified { get; set; }
    }
}