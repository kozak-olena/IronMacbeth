using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using Microsoft.Win32;

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
                if (sellable is Memory)
                {
                    ServerAdapter.Instance.CreateStoreMemory(new StoreMemory { SellableId = sellable.Id, StoreId = Store.Id });
                }
                else if (sellable is Motherboard)
                {
                    ServerAdapter.Instance.CreateStoreMotherboard(new StoreMotherboard { SellableId = sellable.Id, StoreId = Store.Id });
                }
                else if (sellable is Processor)
                {
                    ServerAdapter.Instance.CreateStoreProcessor(new StoreProcessor { SellableId = sellable.Id, StoreId = Store.Id });
                }
                else if (sellable is Videocard)
                {
                    ServerAdapter.Instance.CreateStoreVideocard(new StoreVideocard { SellableId = sellable.Id, StoreId = Store.Id });
                }
                else
                {
                    throw new NotSupportedException($"ISellable of type '{sellable.GetType().FullName}' is not supported.");
                }

                UpdateModifiedLinks();

                UpdateCollectionToAddNoFilter();
                UpdateCollectionAddedNoFilter();
            }
        }

        public void RigthMethod(object parameter)
        {
            ISellableLink sellableLink = SelectedAddedItem as ISellableLink;

            if (sellableLink != null)
            {
                if (sellableLink is StoreMemory)
                {
                    ServerAdapter.Instance.DeleteStoreMemory(sellableLink.Id);
                }
                else if (sellableLink is StoreMotherboard)
                {
                    ServerAdapter.Instance.DeleteStoreMotherboard(sellableLink.Id);
                }
                else if (sellableLink is StoreProcessor)
                {
                    ServerAdapter.Instance.DeleteStoreProcessor(sellableLink.Id);
                }
                else if( sellableLink is StoreVideocard)
                {
                    ServerAdapter.Instance.DeleteStoreVideocard(sellableLink.Id);
                }
                else
                {
                    throw new NotSupportedException($"ISellableLink of type '{sellableLink.GetType().FullName}' is not supported.");
                }


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

                ServerAdapter.Instance.UpdateStore(Store);

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
                ServerAdapter.Instance.CreateStore(Store);
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


            SellableAdded.AddRange
            (
                ServerAdapter.Instance
                    .GetStoreSellableLinks(Store.Id)
                    .Select
                    (
                        link =>
                        {
                            ISellable sellable;

                            if (link is StoreMemory storeMemory)
                            {
                                sellable = ServerAdapter.Instance.GetMemoryFromStoreMemory(storeMemory);
                            }
                            else if (link is StoreMotherboard storeMotherboard)
                            {
                                sellable = ServerAdapter.Instance.GetMotherboardFromStoreMotherboard(storeMotherboard);
                            }
                            else if (link is StoreProcessor storeProcessor)
                            {
                                sellable = ServerAdapter.Instance.GetProcessorFromStoreProcessor(storeProcessor);
                            }
                            else if (link is StoreVideocard storeVideocard)
                            {
                                sellable = ServerAdapter.Instance.GetVideoCardFromStoreVideoCard(storeVideocard);
                            }
                            else
                            {
                                throw new NotSupportedException($"ISellableLink of type '{link.GetType().FullName}' is not supported.");
                            }

                            return new { SellableLink = link, Sellable = sellable };
                        }
                    )
                    .Where(item => item.Sellable.Name.ToLower().Contains(ItemsAddedSearch.ToLower()))
                    .Select(x => x.SellableLink)
            );

            OnPropertyChanged(nameof(SellableAdded));
        }

        public void UpdateCollectionToAdd()
        {
            var storeSellables = ServerAdapter.Instance.GetStoreSellables(Store.Id);

            SellableToAdd = new List<ISellable>();

            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllProcessors().Where(item => !storeSellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));

            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllVideoCards().Where(item => !storeSellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));
            
            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllMotherboards().Where(item => !storeSellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));
            
            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllMemories().Where(item => !storeSellables.Contains(item)).
                                Where(item => item.Name.ToLower().Contains(ItemsToAddSearch.ToLower())));


            OnPropertyChanged(nameof(SellableToAdd));
        }


        public void UpdateCollectionAddedNoFilter()
        {
            SellableAdded = new List<ISellableLink>();

            var storeSellableLinks = ServerAdapter.Instance.GetStoreSellableLinks(Store.Id);

            SellableAdded.AddRange(storeSellableLinks);

            OnPropertyChanged(nameof(SellableAdded));
        }

        public void UpdateCollectionToAddNoFilter()
        {
            var storeSellables = ServerAdapter.Instance.GetStoreSellables(Store.Id);

            SellableToAdd = new List<ISellable>();

            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllProcessors().Where(item => !storeSellables.Contains(item)));
            
            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllVideoCards().Where(item => !storeSellables.Contains(item)));
            
            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllMotherboards().Where(item => !storeSellables.Contains(item)));
            
            SellableToAdd.AddRange(
                ServerAdapter.Instance.GetAllMemories().Where(item => !storeSellables.Contains(item)));

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
            foreach (var link in SellableAdded)
            {
                if (link.Modified)
                {
                    link.Modified = false;

                    if (link is StoreMemory storeMemory)
                    {
                        ServerAdapter.Instance.UpdateStoreMemory(storeMemory);
                    }
                    else if (link is StoreMotherboard storeMotherboard)
                    {
                        ServerAdapter.Instance.UpdateStoreMotherboard(storeMotherboard);
                    }
                    else if (link is StoreProcessor storeProcessor)
                    {
                        ServerAdapter.Instance.UpdateStoreProcessor(storeProcessor);
                    }
                    else if (link is StoreVideocard storeVideocard)
                    {
                        ServerAdapter.Instance.UpdateStoreVideocard(storeVideocard);
                    }
                    else
                    {
                        throw new NotSupportedException($"ISellableLink of type '{link.GetType().FullName}' is not supported.");
                    }
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