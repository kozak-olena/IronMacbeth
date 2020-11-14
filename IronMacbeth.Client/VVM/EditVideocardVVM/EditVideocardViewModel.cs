using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using Microsoft.Win32;

namespace IronMacbeth.Client.VVM.EditVideocardVVM
{
    public class EditVideocardViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Videocard";
        public Videocard Videocard { get; private set; }

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }
        public BitmapImage BitmapImage { get; set; }

        public string Description { get; set; }

        public string Memory { get; set; }
        public string GPUFrequency { get; set; }
        public string MemoryFrequency { get; set; }
        public string Bus { get; set; }

        public string GPU { get; set; }
        public string MemoryType { get; set; }
        public string Interface { get; set; }
        public string Cooling { get; set; }
        public string Model { get; set; }
        public string MPN { get; set; }



        public ICommand CloseCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ApplyChangesCommand { get; set; }

        public EditVideocardViewModel(Videocard videocard = null)
        {
            Description = "";
            Videocard = videocard;
            if (Videocard != null)
            {
                ImagePath = "<image>";
                BitmapImage = Videocard.BitmapImage;

                Memory = Videocard.Memory.ToString();
                GPUFrequency = Videocard.GPUFrequency.ToString();
                MemoryFrequency = Videocard.MemoryFrequency.ToString();
                Bus = Videocard.Bus.ToString();

                GPU = Videocard.GPU;
                MemoryType = Videocard.MemoryType;
                Interface = Videocard.Interface;
                Cooling = Videocard.Cooling;
                Model = Videocard.Model;
                MPN = Videocard.MPN;
                Description = Videocard.Description;
            }

            CloseCommand = new RelayCommand(CloseMethod);
            SelectImageCommand = new RelayCommand(SelectImageMethod);
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod)
            {
                CanExecuteFunc = ApplyChangesCanExecute
            };
        }

        public void ApplyChangesMethod(object parameter)
        {
            if (Videocard != null)
            {
                Videocard.Memory = int.Parse(Memory);
                Videocard.GPUFrequency = int.Parse(GPUFrequency);
                Videocard.MemoryFrequency = int.Parse(MemoryFrequency);
                Videocard.Bus = int.Parse(Bus);

                Videocard.GPU = GPU;
                Videocard.MemoryType = MemoryType;
                Videocard.Interface = Interface;
                Videocard.Cooling = Cooling;
                Videocard.Model = Model;
                Videocard.MPN = MPN;

                if (!Videocard.BitmapImage.Equals(BitmapImage))
                {
                    Videocard.BitmapImage = BitmapImage;
                    Videocard.ImageName = null;
                }
                if (Videocard.Description != Description)
                {
                    Videocard.Description = Description;
                    Videocard.DescriptionName = null;
                }

                ServerAdapter.Instance.UpdateVideoCard(Videocard);
            }
            else
            {
                Videocard = new Videocard
                {
                    BitmapImage = BitmapImage,
                    Memory = int.Parse(Memory),
                    GPUFrequency = int.Parse(GPUFrequency),
                    MemoryFrequency = int.Parse(MemoryFrequency),
                    Bus = int.Parse(Bus),
                    GPU = GPU,
                    MemoryType = MemoryType,
                    Interface = Interface,
                    Cooling = Cooling,
                    Model = Model,
                    MPN = MPN,
                    Description = Description
                };
                ServerAdapter.Instance.CreateVideoCard(Videocard);
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
            return int.TryParse(Memory, out n) &&
                   int.TryParse(GPUFrequency, out n) &&
                   int.TryParse(MemoryFrequency, out n) &&
                   int.TryParse(Bus, out n) &&
                   !string.IsNullOrWhiteSpace(Model) &&
                   !string.IsNullOrWhiteSpace(MPN) &&
                   !string.IsNullOrWhiteSpace(GPU) &&
                   !string.IsNullOrWhiteSpace(MemoryType) &&
                   !string.IsNullOrWhiteSpace(Interface) &&
                   !string.IsNullOrWhiteSpace(Cooling) &&
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