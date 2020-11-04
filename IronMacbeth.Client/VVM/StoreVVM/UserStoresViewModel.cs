using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditStoreVVM;
using IronMacbeth.Model.ToBeRemoved;

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

            MainViewModel.LoadSellable();

            Items = _user.Stores;
            
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
            Store.Items.Remove(parameter as Store);
            MainViewModel.ServerAdapter.Delete(parameter as Store);
            UpdateCollection();
        }

        public void UpdateCollection()
        {
            Store.Items = MainViewModel.ServerAdapter.GetAll<Store>();
            Items = _user.Stores;
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