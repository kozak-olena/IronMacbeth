using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.ViewModel;

namespace IronMacbeth.Client.VVM.PuchaseVVM
{
    public class PurchaseViewModel : IPageViewModel
    {
        private readonly Store _store;
        private readonly ISellable _sellable;
        private readonly ISellableLink _sellableLink;

        public bool Executed { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }

        public BitmapImage MerchandiseImage => _sellable.BitmapImage;
        public string MerchandiseName => _sellable.Name;

        public BitmapImage StoreImage => _store.BitmapImage;
        public string StoreName => _store.Name;

        public ICommand DoneCommand { get; }

        public PurchaseViewModel(Store store, ISellable sellable, ISellableLink sellableLink)
        {
            _store = store;
            _sellable = sellable;
            _sellableLink = sellableLink;

            Executed = false;

            DoneCommand = new RelayCommand(DoneMethod) { CanExecuteFunc = DoneCanExecute };
        }

        public void DoneMethod(object parameter)
        {
            Purchase purchase = new Purchase
            {
                LinkName = _sellableLink.GetType().FullName,
                LinkId = _sellableLink.Id,
                FirstName = FirstName,
                SecondName = SecondName,
                Email = Email,
                Number = int.Parse(Number),
                Date = DateTime.Now.ToLongTimeString(),
                IsMarkedAsRead = false
            };

            MainViewModel.ServerAdapter.CreatePurchase(purchase);

            Executed = true;
        }


        public string PageViewName => "Purchase";

        public void Update() { }

        public bool DoneCanExecute(object parameter)
        {
            int n;
            return int.TryParse(Number, out n) &&
                   !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(SecondName) &&
                   !Executed;
        }
    }
}