using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditMemoryVVM;

namespace IronMacbeth.Client.VVM.MemoryVVM
{
    public class MemoryViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Memory";

        private List<MemoryItemViewModel> _items;

        public List<MemoryItemViewModel> Items
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

        public MemoryViewModel()
        {
            AddCommand = new RelayCommand(AddMethod);
            EditCommand = new RelayCommand(EditMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
            DeleteCommand = new RelayCommand(DeleteMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void AddMethod(object parameter)
        {
            var editMemoryViewModel = new EditMemoryViewModel();
            new EditMemoryWindow { DataContext = editMemoryViewModel }.ShowDialog();
            if (editMemoryViewModel.CollectionChanged)
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
            var editMemoryViewModel = new EditMemoryViewModel(SelectedItem as Memory);
            new EditMemoryWindow { DataContext = editMemoryViewModel }.ShowDialog();
            if (editMemoryViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void DeleteMethod(object parameter)
        {
            if (SelectedItem is Memory memory)
            {
                MainViewModel.ServerAdapter.DeleteMemory(memory.Id);
                Update();
                UpdateCollection(false);
            }
        }

        public void UpdateCollection(bool innerCall)
        {
            _items = 
                MainViewModel.ServerAdapter.GetAllMemories()
                    .OrderByDescending(item => item.NumberOfOfferings)
                    .Where(item => item.Name.ToLower().Contains(Search.ToLower()))
                    .Select(x => new MemoryItemViewModel(x))
                    .ToList();

            if (!innerCall)
            {
                OnPropertyChanged(nameof(Items));
            }
        }

        public void UpdateCollectionNoFilter()
        {
            _items =
                MainViewModel.ServerAdapter.GetAllMemories()
                    .OrderByDescending(item => item.NumberOfOfferings)
                    .Select(x => new MemoryItemViewModel(x))
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