using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using Microsoft.Win32;

namespace IronMacbeth.Client.VVM.EditMemoryVVM
{
    public class EditMemoryViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Memory";
        public Memory Memory { get; private set; }

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }
        public BitmapImage BitmapImage { get; set; }

        public string Description { get; set; }

        public string Size { get; set; }
        public string Frequency { get; set; }

        public string Type { get; set; }
        public string Standart { get; set; }
        public string Timings { get; set; }
        public string Voltage { get; set; }
        public string FormFactor { get; set; }
        public string Model { get; set; }
        public string MPN { get; set; }



        public ICommand CloseCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ApplyChangesCommand { get; set; }

        public EditMemoryViewModel(Memory memory = null)
        {
            Description = "";
            Memory = memory;
            if (Memory != null)
            {
                ImagePath = "<image>";
                BitmapImage = Memory.BitmapImage;

                Size = Memory.Size.ToString();
                Frequency = Memory.Frequency.ToString();

                Type = Memory.Type;
                Standart = Memory.Standart;
                Timings = Memory.Timings;
                Voltage = Memory.Voltage;
                FormFactor = Memory.FormFactor;
                Model = Memory.Model;
                MPN = Memory.MPN;
                Description = Memory.Description;
            }

            CloseCommand = new RelayCommand(CloseMethod);
            SelectImageCommand = new RelayCommand(SelectImageMethod);
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod) { CanExecuteFunc = ApplyChangesCanExecute };
        }

        public void ApplyChangesMethod(object parameter)
        {
            if (Memory != null)
            {
                Memory.Size = int.Parse(Size);
                Memory.Frequency = int.Parse(Frequency);

                Memory.Type = Type;
                Memory.Standart = Standart;
                Memory.Timings = Timings;
                Memory.Voltage = Voltage;
                Memory.FormFactor = FormFactor;
                Memory.Model = Model;
                Memory.MPN = MPN;

                if (!Memory.BitmapImage.Equals(BitmapImage))
                {
                    Memory.BitmapImage = BitmapImage;
                    Memory.ImageName = null;
                }
                if (Memory.Description != Description)
                {
                    Memory.Description = Description;
                    Memory.DescriptionName = null;
                }

                ServerAdapter.Instance.UpdateMemory(Memory);
            }
            else
            {
                Memory = new Memory
                {
                    BitmapImage = BitmapImage,
                    Size = int.Parse(Size),
                    Frequency = int.Parse(Frequency),
                    Type = Type,
                    Standart = Standart,
                    Timings = Timings,
                    Voltage = Voltage,
                    FormFactor = FormFactor,
                    Model = Model,
                    MPN = MPN,
                    Description = Description
                };
                ServerAdapter.Instance.CreateMemory(Memory);
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
            int n;
            return int.TryParse(Size, out n) &&
                   int.TryParse(Frequency, out n) &&
                   !string.IsNullOrWhiteSpace(Model) &&
                   !string.IsNullOrWhiteSpace(MPN) &&
                   !string.IsNullOrWhiteSpace(Type) &&
                   !string.IsNullOrWhiteSpace(Standart) &&
                   !string.IsNullOrWhiteSpace(Timings) &&
                   !string.IsNullOrWhiteSpace(Voltage) &&
                   !string.IsNullOrWhiteSpace(FormFactor) &&
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