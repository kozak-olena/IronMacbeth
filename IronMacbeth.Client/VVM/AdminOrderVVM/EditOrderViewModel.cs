using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IronMacbeth.Client.VVM.AdminOrderVVM
{
    public class EditOrderViewModel : INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; set; }
        public ICommand ApplyChangesCommand { get; set; }

        public string[] AvailibleItemTypes => new[] { "Order is accepted", "Order is in proccessing", "Order completed", "Document is currently taking by the user", "Document returned" };

        public string Status { get; set; }
        public DateTime Min { get; }

        private DateTime _receiveDate;

        public DateTime ReceiveDate
        {
            get { return _receiveDate; }
            set
            {
                _receiveDate = value;
                OnPropertyChanged(nameof(ReceiveDate));
            }
        }


        public DateTime MinDateReturn { get; }


        private DateTime _dateTimeOfReturning;

        public DateTime DateOfReturning
        {
            get { return _dateTimeOfReturning; }
            set
            {
                _dateTimeOfReturning = value;
                OnPropertyChanged(nameof(DateOfReturning));
            }
        }


        public Order CurrentOrder;
        public EditOrderViewModel(Order order)
        {
            CurrentOrder = order;
            DateTime dateTimeOfReceiving = DateTime.Now;
            Min = new DateTime(dateTimeOfReceiving.Ticks - (dateTimeOfReceiving.Ticks % TimeSpan.TicksPerSecond), dateTimeOfReceiving.Kind);
            _receiveDate = order.ReceiveDate;

            DateTime dateTimeOfReturning = order.DateOfReturn;
            MinDateReturn = new DateTime(dateTimeOfReturning.Ticks - (dateTimeOfReturning.Ticks % TimeSpan.TicksPerSecond), dateTimeOfReturning.Kind);
            _dateTimeOfReturning = DateTime.Now;
            CloseCommand = new RelayCommand(CloseMethod);
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void ApplyChangesMethod(object parameter)
        {
            SpecifiedOrderFields SpecifyOrderFields = new SpecifiedOrderFields();
            SpecifyOrderFields.DateOfReturning = DateOfReturning;
            SpecifyOrderFields.ReceiveDate = ReceiveDate;
            SpecifyOrderFields.Status = Status;
            // ServerAdapter.Instance.UpdateOrder(SpecifyOrderFields);
            CloseMethod(parameter);
        }

        public bool CanExecuteMaintenanceMethods(object parameter)
        {
            return Status != null;
        }

        public void CloseMethod(object parameter)
        {
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
