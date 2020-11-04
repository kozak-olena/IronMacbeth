using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditProcessorVVM;
using IronMacbeth.Client.VVM.EditVideocardVVM;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.VideocardVVM
{
    class VideocardViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Videocards";

        private List<Videocard> _items;

        public List<Videocard> Items
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

        public VideocardViewModel()
        {
            Search = "";

            UpdateCollection(false);

            AddCommand = new RelayCommand(AddMethod);
            EditCommand = new RelayCommand(EditMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
            DeleteCommand = new RelayCommand(DeleteMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void AddMethod(object parameter)
        {
            var editVideocardViewModel = new EditVideocardViewModel();
            new EditVideocardWindow { DataContext = editVideocardViewModel }.ShowDialog();
            if (editVideocardViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void Update()
        {
            MainViewModel.LoadSellable();
            //Store.Items = MainViewModel.ServerAdapter.GetAll<Store>();
            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();
            //StoreProcessor.Items = MainViewModel.ServerAdapter.GetAll<StoreProcessor>();
        }

        public void EditMethod(object parameter)
        {
            var editVideocardViewModel = new EditVideocardViewModel(SelectedItem as Videocard);
            new EditVideocardWindow { DataContext = editVideocardViewModel }.ShowDialog();
            if (editVideocardViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void DeleteMethod(object parameter)
        {
            MainViewModel.ServerAdapter.Delete(SelectedItem as Videocard);
            Update();
            UpdateCollection(false);
        }

        public void UpdateCollection(bool innerCall)
        {
            _items = Videocard.Items.
                OrderByDescending(item => item.NumberOfOfferings).
                Where(item => item.Name.ToLower().Contains(Search.ToLower())).ToList();

            if (!innerCall)
            {
                OnPropertyChanged(nameof(Items));
            }
        }

        public void UpdateCollectionNoFilter()
        {
            //StoreProcessor.Items = MainViewModel.ServerAdapter.GetAll<StoreProcessor>();
            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();

            _items = Videocard.Items.
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
