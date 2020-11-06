using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace IronMacbeth.Client
{
    [Serializable]
    public class Purchase : Base,INotifyPropertyChanged
    {
        
        public int Id { get; set; }

        
        public int LinkId { get; set; }

        
        public int Number { get; set; }

        
        public string LinkName { get; set; }

        
        public string FirstName { get; set; }

        
        public string SecondName { get; set; }

        
        public string Email { get; set; }

        
        public string Date
        {
            get { return _dateTime.ToString(); }
            set { _dateTime = DateTime.Parse(value); }
        }

        
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