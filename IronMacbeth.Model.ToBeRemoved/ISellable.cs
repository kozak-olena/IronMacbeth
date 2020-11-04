namespace IronMacbeth.Model.ToBeRemoved
{
    public interface ISellable:IDisplayable,IDescribable
    {
        int Id { get; } 

        string Name { get; }

        string SellableType { get; }
    }
}