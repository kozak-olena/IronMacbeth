using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
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
    class EditBookViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Book";
        public Book Book { get; private set; }

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }

        public string PdfPath { get; set; }

        public BitmapImage BitmapImage { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string PublishingHouse { get; set; }

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }

        public string Availiability { get; set; }   //electronic version???

        public string Location { get; set; }
        public string IssueNumber { get; set; }
        public string RentPrice { get; set; }

        public string TypeOfDocument { get; set; }

        public byte[] ElectronicVersion { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        private string _selecteItemType;
        public string SelectedItemType
        {
            get { return _selecteItemType; }
            set
            {
                OnSelectedItemTypeChanged(value);
                _selecteItemType = value;
            }
        }

        public bool IsBookSelected = false;
        public bool IsArticleSelected = false;
        public bool IsPeriodicalSelected = false;
        public bool IsThesisSelected = false;
        public bool IsNewspaperSelected = false;

        #region Visivility
        public Visibility AuthorItemVisbility => IsBookSelected || IsArticleSelected || IsThesisSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PublishingHouseVisibility => IsBookSelected || IsPeriodicalSelected || IsThesisSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility LocationVisibility => IsBookSelected || IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility RentPriceVisibility => IsBookSelected || IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility MainDocumentVisibility => IsArticleSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IssueNumberVisibility => IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ResponsibleVisibility => IsPeriodicalSelected || IsThesisSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PagesVisibility => IsPeriodicalSelected || IsBookSelected || IsThesisSelected || IsArticleSelected ? Visibility.Visible : Visibility.Collapsed;

        #endregion
        private void OnSelectedItemTypeChanged(string value)
        {
            if (value == "Book")
            {
                IsBookSelected = true;
                OnPropertyChanged(nameof(AuthorItemVisbility));
                OnPropertyChanged(nameof(PublishingHouseVisibility));
                OnPropertyChanged(nameof(LocationVisibility));
                OnPropertyChanged(nameof(RentPriceVisibility));
                OnPropertyChanged(nameof(PagesVisibility));
            }
            else if (value == "Article")
            {
                IsArticleSelected = true;
                OnPropertyChanged(nameof(AuthorItemVisbility));
                OnPropertyChanged(nameof(MainDocumentVisibility));
                OnPropertyChanged(nameof(PagesVisibility));

            }
            else if (value == "Periodical")
            {
                IsPeriodicalSelected = true;
                OnPropertyChanged(nameof(PublishingHouseVisibility));
                OnPropertyChanged(nameof(LocationVisibility));
                OnPropertyChanged(nameof(RentPriceVisibility));
                OnPropertyChanged(nameof(IssueNumberVisibility));
                OnPropertyChanged(nameof(ResponsibleVisibility));
                OnPropertyChanged(nameof(PagesVisibility));
            }
            else if (value == "Thesis")
            {
                IsThesisSelected = true;
                OnPropertyChanged(nameof(PublishingHouseVisibility));
                OnPropertyChanged(nameof(AuthorItemVisbility));
                OnPropertyChanged(nameof(ResponsibleVisibility));
                OnPropertyChanged(nameof(PagesVisibility));
            }
            else if (value == "Newspaper")
            {
                IsNewspaperSelected = true;
                OnPropertyChanged(nameof(LocationVisibility));
                OnPropertyChanged(nameof(RentPriceVisibility));
                OnPropertyChanged(nameof(IssueNumberVisibility));
            }
            else
            {
                throw new NotImplementedException("Selected item is not supported");
            }
        }

        public string[] AvailibleItemTypes => new[] { "Book", "Article", "Periodical", "Thesis", "Newspaper" };

        public ICommand CloseCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand SelectPdfCommand { get; set; }
        public ICommand ApplyChangesCommand { get; set; }



        public EditBookViewModel(Book book = null)
        {
            Name = "";
            Book = book;
            if (Book != null)
            {
                ImagePath = "<image>";
                PdfPath = "<pdf>";
                BitmapImage = Book.BitmapImage;

                Author = Book.Author;
                PublishingHouse = Book.PublishingHouse;
                TypeOfDocument = Book.TypeOfDocument;
                Pages = Book.Pages;
                City = Book.City;
                Year = Book.Year;
                Location = Book.Location;
                
                Availiability = Book.Availiability;
                ElectronicVersion = Book.ElectronicVersion;
                Rating = Book.Rating;
                Comments = Book.Comments;
            }

            CloseCommand = new RelayCommand(CloseMethod);
            SelectImageCommand = new RelayCommand(SelectImageMethod);
            SelectPdfCommand = new RelayCommand(SelectPdfMethod);     //TODO: Select electronic version
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod)
            {
                CanExecuteFunc = ApplyChangesCanExecute
            };
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
                ElectronicVersion = File.ReadAllBytes(PdfPath);
            }
        }

        public void ApplyChangesMethod(object parameter)
        {
            if (Book != null)
            {
                Book.Name = Name;
                Book.Author = Author;

                Book.PublishingHouse = PublishingHouse;                     //}}TODO:????
                Book.City = City;
                Book.Year = Year;
                Book.Pages = Pages;
                Book.Availiability = Availiability;
                Book.Location = Location;
                Book.TypeOfDocument = TypeOfDocument;
                Book.ElectronicVersion = ElectronicVersion;
                Book.Rating = Rating;
                Book.Comments = Comments;

                if (!Book.BitmapImage.Equals(BitmapImage))
                {
                    Book.BitmapImage = BitmapImage;
                    Book.ImageName = null;
                }
                if (Book.Name != Name)
                {
                    Book.Name = Name;
                    Book.DescriptionName = null;
                }

                MainViewModel.ServerAdapter.UpdateBook(Book);   //if article update article
            }
            else
            {
                Book = new Book
                {
                    BitmapImage = BitmapImage,
                    Name = Name,
                    Author = Author,
                    PublishingHouse = PublishingHouse,
                    City = City,
                    Year = Year,
                    Pages = Pages,
                    Availiability = Availiability,
                    Location = Location,
                    TypeOfDocument = TypeOfDocument,
                    ElectronicVersion = ElectronicVersion,
                    Rating = Rating,
                    Comments = Comments
                };
                MainViewModel.ServerAdapter.CreateBook(Book);
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

        public bool ApplyChangesCanExecute(object parameter)
        {

            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Author) &&
                   !string.IsNullOrWhiteSpace(PublishingHouse) &&
                   !string.IsNullOrWhiteSpace(City) &&
                   !string.IsNullOrWhiteSpace(Year) &&
                   !string.IsNullOrWhiteSpace(Pages) &&
                   !string.IsNullOrWhiteSpace(Availiability) &&
                   !string.IsNullOrWhiteSpace(Location) &&
                   !string.IsNullOrWhiteSpace(ImagePath);
        }

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
