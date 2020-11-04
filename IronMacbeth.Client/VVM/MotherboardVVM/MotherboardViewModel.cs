using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditMotherboardVVM;
using IronMacbeth.Client.VVM.EditVideocardVVM;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MotherboardVVM
{
    public class MotherboardViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Motherboards";

        private List<Motherboard> _items;

        public List<Motherboard> Items
        {
            get
            {
                UpdateCollection(true);
                return _items;
            }
            private set { _items = value; }
        }

        private string _search;
        public string Search
        {
            get { return _search; }

            set
            {
                _search = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    UpdateCollection(false);
                }
                else
                {
                    UpdateCollectionNoFilter();
                }

            }
        }

        public object SelectedItem { get; set; }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public MotherboardViewModel()
        {
            Search = "";

            UpdateCollection(false);

            AddCommand = new RelayCommand(AddMethod);
            EditCommand = new RelayCommand(EditMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
            DeleteCommand = new RelayCommand(DeleteMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void AddMethod(object parameter)
        {
            var editMotherboardViewModel = new EditMotherboardViewModel();
            new EditMotherboardWindow { DataContext = editMotherboardViewModel }.ShowDialog();
            if (editMotherboardViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void Update()
        {
            MainViewModel.LoadSellable();
        }

        public void EditMethod(object parameter)
        {
            var editMotherboardViewModel = new EditMotherboardViewModel(SelectedItem as Motherboard);
            new EditMotherboardWindow { DataContext = editMotherboardViewModel }.ShowDialog();
            if (editMotherboardViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void DeleteMethod(object parameter)
        {
            MainViewModel.ServerAdapter.Delete(SelectedItem as Motherboard);
            Update();
            UpdateCollection(false);
        }

        public void UpdateCollection(bool innerCall)
        {
            _items = Motherboard.Items.
                OrderByDescending(item => item.NumberOfOfferings).
                Where(item => item.Name.ToLower().Contains(Search.ToLower())).ToList();

            if (!innerCall)
            {
                OnPropertyChanged(nameof(Items));
            }
        }

        public void UpdateCollectionNoFilter()
        {
            _items = Motherboard.Items.
                OrderByDescending(item => item.NumberOfOfferings).ToList();

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