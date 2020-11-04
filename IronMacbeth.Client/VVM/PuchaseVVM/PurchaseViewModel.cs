using System;
using System.Windows.Input;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.PuchaseVVM
{
    public class PurchaseViewModel : IPageViewModel
    {
        public bool Executed { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }

        public ISellableLink SellableLink { get; }

        public ICommand DoneCommand { get; }

        public PurchaseViewModel(ISellableLink sellableLink)
        {
            Executed = false;

            SellableLink = sellableLink;

            DoneCommand = new RelayCommand(DoneMethod) {CanExecuteFunc = DoneCanExecute};
        }

        public void DoneMethod(object parameter)
        {
            Purchase purchase = new Purchase
            {
                LinkName = SellableLink.GetType().FullName,
                LinkId = SellableLink.Id,
                FirstName = FirstName,
                SecondName = SecondName,
                Email = Email,
                Number = int.Parse(Number),
                Date = DateTime.Now.ToLongTimeString(),
                IsMarkedAsRead = false
            };

            MainViewModel.ServerAdapter.Insert(purchase);

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