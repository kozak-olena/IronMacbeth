using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditStoreVVM;

namespace IronMacbeth.Client.VVM.StoreVVM
{
    public class UserStoresViewModel:INotifyPropertyChanged
    {
        public List<Store> Items { get; set; }

        private readonly User _user;

        public ICommand EditCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddStoreCommand { get; }

        public UserStoresViewModel(User user)
        {
            _user = user;

            Items = ServerAdapter.Instance.GetUserStores(_user);
            
            EditCommand = new RelayCommand(EditMethod);
            CloseCommand = new RelayCommand(CloseMethod);
            DeleteCommand = new RelayCommand(DeleteMethod);
            AddStoreCommand = new RelayCommand(AddStoreMethod);
        }

        public void CloseMethod(object parameter)
        {
            (parameter as Window)?.Close();
        }

        public void AddStoreMethod(object parameter)
        {
            StoreEditViewModel storeEditViewModel = new StoreEditViewModel(_user);
            new StoreEditWindow { DataContext = storeEditViewModel }.ShowDialog();
            if (storeEditViewModel.CollectionChanged)
            {
                UpdateCollection();
            }
        }

        public void EditMethod(object parameter)
        {
            StoreEditViewModel storeEditViewModel = new StoreEditViewModel(_user, parameter as Store);
            new StoreEditWindow { DataContext = storeEditViewModel }.ShowDialog();
            if (storeEditViewModel.CollectionChanged)
            {
                UpdateCollection();
            }
        }

        public void DeleteMethod(object parameter)
        {
            if (parameter is Store store)
            {
                ServerAdapter.Instance.DeleteStore(store.Id);
                UpdateCollection();
            }
        }

        public void UpdateCollection()
        {
            Items = ServerAdapter.Instance.GetUserStores(_user);
            OnPropertyChanged(nameof(Items));
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