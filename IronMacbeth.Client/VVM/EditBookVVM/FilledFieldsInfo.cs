using IronMacbeth.Client.Annotations;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Image = IronMacbeth.Client.Model.Image;

namespace IronMacbeth.Client.VVM.EditBookVVM
{
    public class FilledFieldsInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public string Topic { get; set; }

        public string PublishingHouse { get; set; }

        public string MainDocumentId { get; set; }


        public string City { get; set; }

        public int? Year { get; set; }

        public int? Pages { get; set; }

        public string Responsible { get; set; }

        public int? Availiability { get; set; }

        public Guid? ElectronicVersionFileId { get; set; }

        public string Location { get; set; }

        public int? IssueNumber { get; set; }

        public string RentPrice { get; set; }

        public MemoryStream ElectronicVersion { get; set; }


        public string ImagePath { get; set; }
        public Guid? ImageFileId { get; set; }
        public Image Image { get; set; }

        private string _typeOfDocument;
        public string TypeOfDocument
        {
            get { return _typeOfDocument; }
            set
            {
                _typeOfDocument = value;
                OnSelectedItemTypeChanged(value);

            }
        }

        public bool IsBookSelected = false;
        public bool IsArticleSelected = false;
        public bool IsPeriodicalSelected = false;
        public bool IsThesisSelected = false;
        public bool IsNewspaperSelected = false;

        #region Visivility
        public Visibility AuthorItemVisbility => IsBookSelected || IsArticleSelected || IsThesisSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PublishingHouseVisibility => IsBookSelected || IsPeriodicalSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility LocationVisibility => IsBookSelected || IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility RentPriceVisibility => IsBookSelected || IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility MainDocumentVisibility => IsArticleSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IssueNumberVisibility => IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ResponsibleVisibility => IsPeriodicalSelected || IsThesisSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PagesVisibility => IsPeriodicalSelected || IsBookSelected || IsThesisSelected || IsArticleSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ImageVisibility => IsPeriodicalSelected || IsBookSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ToAllVisibility => IsPeriodicalSelected || IsBookSelected || IsThesisSelected || IsArticleSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility CityVisibility => IsPeriodicalSelected || IsBookSelected || IsThesisSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility AvailibilityVisibility => IsBookSelected || IsPeriodicalSelected || IsNewspaperSelected ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ElectronicVersionVisibility => ElectronicVersion != null ? Visibility.Visible : Visibility.Collapsed;
        private void OnSelectedItemTypeChanged(string value)
        {
            if (value == "Book")
            {
                IsBookSelected = true;
                IsArticleSelected = false;
                IsPeriodicalSelected = false;
                IsNewspaperSelected = false;
                IsThesisSelected = false;

            }

            else if (value == "Article")
            {
                IsArticleSelected = true;
                IsBookSelected = false;
                IsPeriodicalSelected = false;
                IsNewspaperSelected = false;
                IsThesisSelected = false;


            }
            else if (value == "Periodical")
            {
                IsPeriodicalSelected = true;
                IsArticleSelected = false;
                IsBookSelected = false;
                IsNewspaperSelected = false;
                IsThesisSelected = false;

            }
            else if (value == "Thesis")
            {
                IsThesisSelected = true;
                IsPeriodicalSelected = false;
                IsArticleSelected = false;
                IsBookSelected = false;
                IsNewspaperSelected = false;

            }
            else if (value == "Newspaper")
            {
                IsNewspaperSelected = true;
                IsThesisSelected = false;
                IsPeriodicalSelected = false;
                IsArticleSelected = false;
                IsBookSelected = false;

            }
            else
            {
                throw new NotImplementedException("Selected item is not supported");
            }
            OnPropertyChanged(nameof(AuthorItemVisbility));
            OnPropertyChanged(nameof(PublishingHouseVisibility));
            OnPropertyChanged(nameof(RentPriceVisibility));
            OnPropertyChanged(nameof(IssueNumberVisibility));
            OnPropertyChanged(nameof(ResponsibleVisibility));
            OnPropertyChanged(nameof(LocationVisibility));
            OnPropertyChanged(nameof(PagesVisibility));
            OnPropertyChanged(nameof(MainDocumentVisibility));
            OnPropertyChanged(nameof(ImageVisibility));
            OnPropertyChanged(nameof(CityVisibility));
            OnPropertyChanged(nameof(AvailibilityVisibility));
        }

        #endregion



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

