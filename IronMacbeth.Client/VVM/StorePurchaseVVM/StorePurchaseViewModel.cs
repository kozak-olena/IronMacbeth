using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;

namespace IronMacbeth.Client.VVM
{
    class StorePurchaseViewModel : INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; }
        public ICommand DeleteCommand { get; }

        public List<Purchase> Items { get; private set; }

        private User _user;

        public StorePurchaseViewModel(User user)
        {
            _user = user;

            CloseCommand = new RelayCommand(CloseMethod);
            DeleteCommand = new RelayCommand(DeleteMethod);

            UpdateCollection();
        }

        public void DeleteMethod(object parameter)
        {
            if (parameter is Purchase purchase)
            {
                MainViewModel.ServerAdapter.DeletePurchase(purchase.Id);
                UpdateCollection();
            }
        }

        private void UpdateCollection()
        {
            Items = new List<Purchase>();
            Items.AddRange(
                MainViewModel.ServerAdapter.GetAllPurchases().
                    Where(item => MainViewModel.ServerAdapter.GetUserStores(_user).Select(x => x.Id).Contains(MainViewModel.ServerAdapter.GetStoreFromPurchase(item).Id)).
                    OrderByDescending(item => item.Date)
            );
            OnPropertyChanged(nameof(Items));
        }

        public void CloseMethod(object parameter)
        {
            foreach (var item in Items.Where(item => item.Modified))
            {
                MainViewModel.ServerAdapter.UpdatePurchase(item);
            }

            (parameter as Window)?.Close();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}