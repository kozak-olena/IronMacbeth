namespace IronMacbeth.Model.ToBeRemoved
{
    public interface ISellable:IDisplayable,IDescribable   //IRentable
    {
        int Id { get; } 

        string Name { get; }

        string SellableType { get; }
    }
}