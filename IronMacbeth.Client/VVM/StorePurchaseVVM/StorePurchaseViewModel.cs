using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM
{
    class StorePurchaseViewModel:INotifyPropertyChanged
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

            MainViewModel.LoadSellable();
                 
            UpdateCollection();
        }

        public void DeleteMethod(object parameter)
        {
            MainViewModel.ServerAdapter.Delete(parameter as Purchase);
            Purchase.Items.Remove(parameter as Purchase);
            UpdateCollection();
        }

        private void UpdateCollection()
        {
            Purchase.Items = MainViewModel.ServerAdapter.GetAll<Purchase>();
            Items = new List<Purchase>();
            Items.AddRange(
                Purchase.Items.
                    Where(item => _user.Stores.Contains(item.Store)).
                    OrderByDescending(item=>item.Date)
            );
            OnPropertyChanged(nameof(Items));
        }

        public void CloseMethod(object parameter)
        {
            foreach (var item in Items.Where(item=>item.Modified))
            {
                MainViewModel.ServerAdapter.Update(item);
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