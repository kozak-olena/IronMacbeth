using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Purchase : Base<Purchase>,INotifyPropertyChanged
    {
        [Database]
        public int Id { get; set; }

        [Database]
        public int LinkId { get; set; }

        [Database]
        public int Number { get; set; }

        [Database]
        public string LinkName { get; set; }

        [Database]
        public string FirstName { get; set; }

        [Database]
        public string SecondName { get; set; }

        [Database]
        public string Email { get; set; }

        [Database]
        public string Date
        {
            get { return _dateTime.ToString(); }
            set { _dateTime = DateTime.Parse(value); }
        }

        [Database]
        public string IsRead
        {
            get { return IsMarkedAsRead.ToString(); }
            set { IsMarkedAsRead = bool.Parse(value); }
        }


        public bool IsMarkedAsRead
        {
            get { return _isRead; }
            set
            {
                Modified = true;
                _isRead = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        private bool _isRead;

        private DateTime _dateTime;

        public override string DisplayString => "Purchase";

        public SolidColorBrush Background =>
            IsMarkedAsRead
                ? new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                : new SolidColorBrush(Color.FromArgb(40, 255, 0, 0));

        public Visibility IsReadVisibility =>
            IsMarkedAsRead
                ? Visibility.Collapsed
                : Visibility.Visible;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}