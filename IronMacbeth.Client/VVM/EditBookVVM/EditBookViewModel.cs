using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditBookVVM;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client.VVM.BookVVM
{
    public class EditBookViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Book";

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }

        public string PdfPath { get; set; }

        //public static BitmapImage BitmapImage { get; set; }

        public FilledFieldsInfo FilledFieldsInfo { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SelectImageCommand { get; set; }

        public ICommand SelectPdfCommand { get; set; }

        public ICommand ApplyChangesCommand { get; set; }

        private Dispatch _dispatch;

        private object _objectForEdit;

        public string[] AvailibleItemTypes => new[] { "Book", "Article", "Periodical", "Thesis", "Newspaper" };

        public EditBookViewModel(object objectForEdit)
        {
            _dispatch = new Dispatch(new IHandler[] { new BookHandler(), new ArticleHandler(), new PeriodicalHandler(), new ThesisHandler(), new NewspaperHandler() });
            _objectForEdit = objectForEdit;
            if (_objectForEdit != null)
            {
                FilledFieldsInfo = _dispatch.UnwrapObjectForEdit(objectForEdit);
            }
            else
            {
                FilledFieldsInfo = new FilledFieldsInfo();
            }
            CloseCommand = new RelayCommand(CloseMethod);
            SelectImageCommand = new RelayCommand(SelectImageMethod);
            SelectPdfCommand = new RelayCommand(SelectPdfMethod);
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod);
            //{
            //    CanExecuteFunc = ApplyChangesCanExecute
            //};
        }

        public void SelectPdfMethod(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "pdf Files (.pdf)|*.pdf",
                FilterIndex = 1,
                Multiselect = false
            };

            bool? userClickedOk = openFileDialog.ShowDialog();

            if (userClickedOk == true)
            {
                PdfPath = openFileDialog.FileName;
                OnPropertyChanged(nameof(PdfPath));
                FilledFieldsInfo.ElectronicVersion = File.ReadAllBytes(PdfPath);       //TODO???
            }
        }

        public void ApplyChangesMethod(object parameter)
        {
            if (_objectForEdit == null)
            {
                _dispatch.DispatchCreation(FilledFieldsInfo);
            }
            else
            {
                _dispatch.DispatchUpdate(FilledFieldsInfo, _objectForEdit);
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

               FilledFieldsInfo.BitmapImage = new BitmapImage(new Uri(ImagePath));
                OnPropertyChanged(nameof(BitmapImage));
            }
        }

        //public bool ApplyChangesCanExecute(object parameter)
        //{
        //    return !string.IsNullOrWhiteSpace(Name) &&
        //           !string.IsNullOrWhiteSpace(City) &&
        //           !string.IsNullOrWhiteSpace(Year) &&
        //           !string.IsNullOrWhiteSpace(Pages) &&
        //           !string.IsNullOrWhiteSpace(Availiability) &&
        //           !string.IsNullOrWhiteSpace(ImagePath);
        //}

        public void Update() { }

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
