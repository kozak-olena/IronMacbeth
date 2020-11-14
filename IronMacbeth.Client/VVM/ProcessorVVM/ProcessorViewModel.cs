using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditProcessorVVM;
using IronMacbeth.Client.VVM.MemoryVVM;

namespace IronMacbeth.Client.VVM.ProcessorVVM
{
    class ProcessorViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Processors";

        private List<ProcessorItemViewModel> _items;

        public List<ProcessorItemViewModel> Items
        {
            get
            {
                UpdateCollection(true);
                return _items;
            }
            private set { _items = value; }
        }

        private string _search = "";
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

        public ProcessorViewModel()
        {
            AddCommand = new RelayCommand(AddMethod);
            EditCommand = new RelayCommand(EditMethod) {CanExecuteFunc = CanExecuteMaintenanceMethods};
            DeleteCommand = new RelayCommand(DeleteMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void AddMethod(object parameter)
        {
            var editProcessorViewModel = new EditProcessorViewModel();
            new EditProcessorWindow { DataContext = editProcessorViewModel }.ShowDialog();
            if (editProcessorViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void Update()
        {

        }

        public void EditMethod(object parameter)
        {
            var editProcessorViewModel = new EditProcessorViewModel(SelectedItem as Processor);
            new EditProcessorWindow { DataContext = editProcessorViewModel }.ShowDialog();
            if (editProcessorViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void DeleteMethod(object parameter)
        {
            if (SelectedItem is Processor processor)
            {
                ServerAdapter.Instance.DeleteProcessor(processor.Id);
                Update();
                UpdateCollection(false);
            }
        }

        public void UpdateCollection(bool innerCall)
        {
            _items = 
                ServerAdapter.Instance.GetAllProcessors()
                    .OrderByDescending(item=>item.NumberOfOfferings)
                    .Where(item => item.Name.ToLower().Contains(Search.ToLower()))
                    .Select(x => new ProcessorItemViewModel(x))
                    .ToList();

            if (!innerCall)
            {
                OnPropertyChanged(nameof(Items));
            }
        }

        public void UpdateCollectionNoFilter()
        {
            _items =
                ServerAdapter.Instance.GetAllProcessors()
                    .OrderByDescending(item => item.NumberOfOfferings)
                    .Select(x => new ProcessorItemViewModel(x))
                    .ToList();

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
