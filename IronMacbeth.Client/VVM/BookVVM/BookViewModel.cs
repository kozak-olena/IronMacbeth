﻿using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditBookVVM;
using IronMacbeth.Client.VVM.EditMemoryVVM;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IronMacbeth.Client.VVM.BookVVM
{
    class BookViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Book";

        private List<BookItemViewModel> _items;

        public List<BookItemViewModel> Items
        {
            get
            {
                UpdateCollection(true);
                return _items;
            }
            private set { _items = value; }
        }

        private string _search = "";     //TODO:??????
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

        public BookViewModel()
        {
            AddCommand = new RelayCommand(AddMethod);
            EditCommand = new RelayCommand(EditMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
            DeleteCommand = new RelayCommand(DeleteMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void AddMethod(object parameter)
        {
            var editBookViewModel = new EditBookViewModel();
            new EditBookWindow2 { DataContext = editBookViewModel }.ShowDialog();
            if (editBookViewModel.CollectionChanged)
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
            var editBookViewModel = new EditBookViewModel(SelectedItem as Book);    
            new EditBookWindow2 { DataContext = editBookViewModel }.ShowDialog();
            if (editBookViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void DeleteMethod(object parameter)
        {
            if (SelectedItem is Book book)
            {
                MainViewModel.ServerAdapter.DeleteBook(book.Id);
                Update();
                UpdateCollection(false);
            }
        }

        public void UpdateCollection(bool innerCall)
        {
            _items =
                MainViewModel.ServerAdapter.GetAllBooks()
                    .OrderByDescending(item => item.NumberOfOfferings)
                    .Where(item => item.Name.ToLower().Contains(Search.ToLower()))
                    .Select(x => new BookItemViewModel(x))
                    .ToList();

            if (!innerCall)
            {
                OnPropertyChanged(nameof(Items));
            }
        }

        public void UpdateCollectionNoFilter()
        {
            _items =
                MainViewModel.ServerAdapter.GetAllBooks()
                    .OrderByDescending(item => item.NumberOfOfferings)
                    .Select(x => new BookItemViewModel(x))
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

