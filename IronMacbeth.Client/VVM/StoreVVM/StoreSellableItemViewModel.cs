using System.Windows.Media.Imaging;
using IronMacbeth.Client.VVM.PuchaseVVM;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.StoreVVM
{
    public class StoreSellableItemViewModel
    {
        private readonly Store _store;
        private readonly ISellable _sellable;
        private readonly ISellableLink _sellableLink;

        public StoreSellableItemViewModel(Store store, ISellable sellable, ISellableLink sellableLink)
        {
            _store = store;
            _sellable = sellable;
            _sellableLink = sellableLink;
        }

        public BitmapImage BitmapImage => _store.BitmapImage;

        public string Name => _store.Name;
        public string Delivery => _store.Delivery;
        public int ProductPrice => _sellableLink.ProductPrice;
        public int ProductWarranty => _sellableLink.ProductWarranty;

        public PurchaseViewModel PurchaseVm => new PurchaseViewModel(_store, _sellable, _sellableLink);
    }
}