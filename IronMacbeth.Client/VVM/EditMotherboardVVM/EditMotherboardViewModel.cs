using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using Microsoft.Win32;

namespace IronMacbeth.Client.VVM.EditMotherboardVVM
{
    public class EditMotherboardViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Motherboard";
        public Motherboard Motherboard { get; private set; }

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }
        public BitmapImage BitmapImage { get; set; }

        public string Description { get; set; }

        public string DIMM { get; set; }
        public string LAN { get; set; }
        public string USB { get; set; }

        public string CPUSocket { get; set; }
        public string Northbridge { get; set; }
        public string Southbridge { get; set; }
        public string GraphicalInterface { get; set; }
        public string Model { get; set; }
        public string MPN { get; set; }



        public ICommand CloseCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ApplyChangesCommand { get; set; }

        public EditMotherboardViewModel(Motherboard motherboard = null)
        {
            Description = "";
            Motherboard = motherboard;
            if (Motherboard != null)
            {
                ImagePath = "<image>";
                BitmapImage = Motherboard.BitmapImage;

                DIMM = Motherboard.DIMM.ToString();
                LAN = Motherboard.LAN.ToString();
                USB = Motherboard.USB.ToString();

                CPUSocket = Motherboard.CPUSocket;
                Northbridge = Motherboard.Northbridge;
                Southbridge = Motherboard.Southbridge;
                GraphicalInterface = Motherboard.GraphicalInterface;
                Model = Motherboard.Model;
                MPN = Motherboard.MPN;
                Description = Motherboard.Description;
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
            if (Motherboard != null)
            {
                Motherboard.DIMM = int.Parse(DIMM);
                Motherboard.LAN = int.Parse(LAN);
                Motherboard.USB = int.Parse(USB);

                Motherboard.CPUSocket = CPUSocket;
                Motherboard.Northbridge = Northbridge;
                Motherboard.Southbridge = Southbridge;
                Motherboard.GraphicalInterface = GraphicalInterface;
                Motherboard.Model = Model;
                Motherboard.MPN = MPN;

                if (!Motherboard.BitmapImage.Equals(BitmapImage))
                {
                    Motherboard.BitmapImage = BitmapImage;
                    Motherboard.ImageName = null;
                }
                if (Motherboard.Description != Description)
                {
                    Motherboard.Description = Description;
                    Motherboard.DescriptionName = null;
                }

                MainViewModel.ServerAdapter.UpdateMotherboard(Motherboard);
            }
            else
            {
                Motherboard = new Motherboard
                {
                    BitmapImage = BitmapImage,
                    DIMM = int.Parse(DIMM),
                    LAN = int.Parse(LAN),
                    USB = int.Parse(USB),
                    CPUSocket = CPUSocket,
                    Northbridge = Northbridge,
                    Southbridge = Southbridge,
                    GraphicalInterface = GraphicalInterface,
                    Model = Model,
                    MPN = MPN,
                    Description = Description
                };
                MainViewModel.ServerAdapter.CreateMotherboard(Motherboard);
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
            return int.TryParse(DIMM, out n) &&
                   int.TryParse(LAN, out n) &&
                   int.TryParse(USB, out n) &&
                   !string.IsNullOrWhiteSpace(Model) &&
                   !string.IsNullOrWhiteSpace(MPN) &&
                   !string.IsNullOrWhiteSpace(CPUSocket) &&
                   !string.IsNullOrWhiteSpace(Northbridge) &&
                   !string.IsNullOrWhiteSpace(Southbridge) &&
                   !string.IsNullOrWhiteSpace(GraphicalInterface) &&
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