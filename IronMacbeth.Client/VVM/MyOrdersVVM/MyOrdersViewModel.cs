using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditBookVVM;
using IronMacbeth.Client.VVM.MyOrdersVVM.MyOrdersItemsVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IronMacbeth.Client.VVM.MyOrdersVVM
{
    class MyOrdersViewModel : IPageViewModel, INotifyPropertyChanged
    {

        public string PageViewName => "My orders";

        public List<IDocumentViewModel> Items { get; private set; }

        public void Update() { ShowCollection(); }
        public ICommand CancelCommand { get; }

        public MyOrdersViewModel()
        {
            CancelCommand = new RelayCommand(CancelMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public IDocumentViewModel SelectedItem { get; set; }

        private object _selectedItem;

        public void CancelMethod(object parameter)
        {
            _selectedItem = SelectedItem.GetItem();
            if (_selectedItem is Order)
            {
                Order orderToDelete = (Order)_selectedItem;
                ServerAdapter.Instance.DeleteOrder(orderToDelete.Id);
                Update();
            }
            else if (_selectedItem is ReadingRoomOrder) 
            {

            }

        }

        public void ShowCollection()
        {
            Items = new List<IDocumentViewModel>();
            List<Order> orders = ServerAdapter.Instance.GetAllOrders();
            Items.AddRange(orders.Select(x => new OrderBookItemViewModel(x)));
            OnPropertyChanged(nameof(Items));
        }

        public bool CanExecuteMaintenanceMethods(object parameter)
        {
            return SelectedItem != null;
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
