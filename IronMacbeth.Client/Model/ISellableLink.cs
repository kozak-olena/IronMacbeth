namespace IronMacbeth.Client
{
    public interface ISellableLink
    {
        int Id { get; set; }

        int StoreId { get; set; }

        int SellableId { get; set; }

        int ProductPrice { get; set; }
        int ProductWarranty { get; set; }

        bool Modified { get; set; }
    }
}