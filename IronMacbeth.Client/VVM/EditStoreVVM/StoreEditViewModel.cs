using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using Microsoft.Win32;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.EditStoreVVM
{
    public class StoreEditViewModel : INotifyPropertyChanged
    {

        public Store Store { get; private set; }

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Delivery { get; set; }

        public BitmapImage BitmapImage { get; set; }

        public ICommand LeftCommand { get; }
        public ICommand RightCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand ApplyChangesCommand { get; }

        public List<ISellableLink> SellableAdded { get; set; }
        public List<ISellable> SellableToAdd { get; set; }

        public object SelectedAddedItem { get; set; }
        public object SelectedToAddItem { get; set; }

        private string _itemsAddedSearch;
        private string _itemsToAddSearch;

        public string ItemsAddedSearch
        {
            get { return _itemsAddedSearch; }
            set
            {
                _itemsAddedSearch = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    UpdateCollectionAdded();
                }
                else
                {
                    UpdateCollectionAddedNoFilter();
                }
            }
        }

        public string ItemsToAddSearch
        {
            get { return _itemsToAddSearch; }
            set
            {
                _itemsToAddSearch = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    UpdateCollectionToAdd();
                }
                else
                {
                    UpdateCollectionToAddNoFilter();
                }
            }
        }

        public Visibility MerchandiseEditEnabled => (Store != null) ? Visibility.Visible : Visibility.Collapsed;

        private readonly User _user;

        public StoreEditViewModel(User user, Store store = null)
        {
            _user = user;
            Store = store;
            if (Store != null)
            {
                ImagePath = "<image>";
                Name = Store.Name;
                Delivery = Store.Delivery;
                BitmapImage = Store.BitmapImage;

                MainViewModel.LoadSellable();

                SellableAdded = new List<ISellableLink>();
                SellableToAdd = new List<ISellable>();

                ItemsToAddSearch = "";
                ItemsAddedSearch = "";
            }

            LeftCommand = new RelayCommand(LeftMethod) { CanExecuteFunc = LeftCanExecute };
            RightCommand = new RelayCommand(RigthMethod) { CanExecuteFunc = RightCanExecute };
            CloseCommand = new RelayCommand(CloseMethod);
            SelectImageCommand = new RelayCommand(SelectImageMethod);
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod)
            {
                CanExecuteFunc = ApplyChangesCanExecute
            };
        }

        public void LeftMethod(object parameter)
        {
            ISellable sellable = SelectedToAddItem as ISellable;

            if (sellable != null)
            {

                StoreSellable sellableLink = new StoreSellable
                {
                    Sellable = sellable,
                    SellableId = sellable.Id,
                    StoreId = Store.Id
                };

                MainViewModel.ServerAdapter.InsertStoreSellable(sellableLink);

                UpdateModifiedLinks();

                MainViewModel.LoadSellableLinks();
                //StoreProcessor.Items = MainViewModel.ServerAdapter.GetAll<StoreProcessor>();

                UpdateCollectionToAddNoFilter();
                UpdateCollectionAddedNoFilter();
            }
        }

        public void RigthMethod(object parameter)
        {
            ISellableLink sellableLink = SelectedAddedItem as ISellableLink;

            if (sellableLink != null)
            {
                MainViewModel.ServerAdapter.DeleteLink(sellableLink);

                MainViewModel.LoadSellableLinks();
                //StoreProcessor.Items = MainViewModel.ServerAdapter.GetAll<StoreProcessor>();

                UpdateCollectionToAddNoFilter();
                UpdateCollectionAddedNoFilter();
            }
        }

        public bool LeftCanExecute(object parameter)
        {
            return SelectedToAddItem != null;
        }
        public bool RightCanExecute(object parameter)
        {
            return SelectedAddedItem != null;
        }

        public void ApplyChangesMethod(object parameter)
        {
            if (Store != null)
            {
                Store.Name = Name;
                Store.Delivery = Delivery;
                if (!Store.BitmapImage.Equals(BitmapImage))
                {
                    Store.BitmapImage = BitmapImage;
                    Store.ImageName = null;
                }

                MainViewModel.ServerAdapter.Update(Store);

                UpdateModifiedLinks();
            }
            else
            {
                Store = new Store
                {
                    Name = Name,
                    Delivery = Delivery,
                    BitmapImage = BitmapImage,
                    OwnerId = _user.Login
                };
                MainViewModel.ServerAdapter.Insert(Store);
            }

            CollectionChanged = true;

            CloseMethod(parameter);
        }

        public void CloseMethod(object parameter)
        {
            (parameter as Window)?.Close();
        }

        public void SelectImageMethod(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (.png)|*.png",
                FilterIndex = 1,
                Multiselect = false
            };

            bool? userClickedOk = openFileDialog.ShowDialog();

            if (userClickedOk == true)
            {
                ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(ImagePath));

                BitmapImage = new BitmapImage(new Uri(ImagePath));
                OnPropertyChanged(nameof(BitmapImage));
            }
        }

        public void UpdateCollectionAdded()
        {
            SellableAdded = new List<ISellableLink>();

            SellableAdded.AddRange(Store.SellableLinks.Where(item => item.Sellable.Name.ToLower().Contains(ItemsAddedSearch.ToLower())));

            OnPropertyChanged(nameof(SellableAdded));
        }

        public void UpdateCollectionToAdd()
        {
            SellableToAdd = new List<ISellable>();

            SellableToAdd.AddRange(
                Processor.Items.Where(item => !Store.Sellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));
            SellableToAdd.AddRange(
                Videocard.Items.Where(item => !Store.Sellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));

            SellableToAdd.AddRange(
                Motherboard.Items.Where(item => !Store.Sellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));

            SellableToAdd.AddRange(
                Memory.Items.Where(item => !Store.Sellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));


            OnPropertyChanged(nameof(SellableToAdd));
        }


        public void UpdateCollectionAddedNoFilter()
        {
            SellableAdded = new List<ISellableLink>();

            SellableAdded.AddRange(Store.SellableLinks);

            OnPropertyChanged(nameof(SellableAdded));
        }

        public void UpdateCollectionToAddNoFilter()
        {
            SellableToAdd = new List<ISellable>();

            SellableToAdd.AddRange(
                Processor.Items.Where(item => !Store.Sellables.Contains(item)));

            SellableToAdd.AddRange(
                Videocard.Items.Where(item => !Store.Sellables.Contains(item)));

            SellableToAdd.AddRange(
                Motherboard.Items.Where(item => !Store.Sellables.Contains(item)));

            SellableToAdd.AddRange(
                Memory.Items.Where(item => !Store.Sellables.Contains(item)));

            OnPropertyChanged(nameof(SellableToAdd));
        }

        public bool ApplyChangesCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Delivery) &&
                   !string.IsNullOrWhiteSpace(ImagePath);
        }

        private void UpdateModifiedLinks()
        {
            foreach (var link in Store.SellableLinks)
            {
                if (link.Modified)
                {
                    link.Modified = false;
                    MainViewModel.ServerAdapter.Update(link);
                }
            }
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