using IronMacbeth.BFF.Contract;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.VVM.AdminOrderVVM;
using IronMacbeth.Client.VVM.BookVVM;
using IronMacbeth.Client.VVM.Home;
using IronMacbeth.Client.VVM.LogInVVM;
using IronMacbeth.Client.VVM.LogInVVM.RegisterVVM;
using IronMacbeth.Client.VVM.MyOrdersVVM;
using IronMacbeth.Client.VVM.SearchPageViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace IronMacbeth.Client.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public User User => UserService.LoggedInUser;
        private bool UserLoggedIn => User != null;


        public ICommand BackCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand LogInCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ForwardCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ReconnectCommand { get; }
        public ICommand ChangePageCommand { get; }

        private IPageViewModel _currentPageViewModel;

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        public string Login => User?.Login;

        public Visibility MenuVisibility => UserLoggedIn ? Visibility.Visible : Visibility.Collapsed;
        public Visibility AdminToolsVisibility
        {
            get
            {
                if (User != null && User.IsAdmin)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public List<IPageViewModel> PageViewModels { get; set; }


        private readonly Stack<IPageViewModel> _previousPages;
        private readonly Stack<IPageViewModel> _nextPages;

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                if (ConnectionMenuShown && value)
                {
                    ConnectionMenuText = "Connected to server";
                    ConnectionMenuHideRequired = true;
                }
                if (!value && !ReconnectButtonShown)
                {
                    ConnectionMenuText = "Not connected to server";
                    LogOutMethod(null);
                    ReconnectButtonShown = true;
                }
                if (value && ReconnectButtonShown)
                {
                    ReconnectButtonShown = false;
                }
                if (!value && !ConnectionMenuShown)
                {
                    ConnectionMenuShown = true;
                }

                _connected = value;

                // Forces view to re-check all CanExecute statuses
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool ConnectionMenuShown { get; set; }

        private bool _connectionMenuHideRequired;
        public bool ConnectionMenuHideRequired
        {
            get { return _connectionMenuHideRequired; }
            set
            {
                ConnectionMenuShown = false;
                _connectionMenuHideRequired = value;
                OnPropertyChanged(nameof(ConnectionMenuHideRequired));
            }
        }

        private bool _reconnectButtonShown;
        public bool ReconnectButtonShown
        {
            get { return _reconnectButtonShown; }
            set
            {
                _reconnectButtonShown = value;
                OnPropertyChanged(nameof(ReconnectButtonShown));
            }
        }

        private string _connectionMenuText;
        public string ConnectionMenuText
        {
            get { return _connectionMenuText; }
            set
            {
                _connectionMenuText = value;
                OnPropertyChanged(nameof(ConnectionMenuText));
            }
        }

        private Timer connectionCheckTimer;

        private Task<bool> checkTask = new Task<bool>(() =>
        {
            try
            {
                return AnonymousServerAdapter.Instance.Ping();
            }
            catch
            {
                return false;
            }
        });


        public MainViewModel()
        {
            connectionCheckTimer = new Timer();
            connectionCheckTimer.Interval = 10000;
            connectionCheckTimer.Elapsed += ConnectionCheck;
            connectionCheckTimer.Start();

            ConnectAsync();

            BackCommand = new RelayCommand(BackMethod);
            CloseCommand = new RelayCommand(CloseMethod);
            LogInCommand = new RelayCommand(LogInMethod) { CanExecuteFunc = CanExecuteAutorizationMethods };
            LogOutCommand = new RelayCommand(LogOutMethod) { CanExecuteFunc = CanExecuteLogOutMethod };
            ForwardCommand = new RelayCommand(ForwardMethod);
            RegisterCommand = new RelayCommand(RegisterMethod) { CanExecuteFunc = CanExecuteAutorizationMethods };
            ReconnectCommand = new RelayCommand(ReconnectMethod);
            ChangePageCommand = new RelayCommand(ChangePageMethod);
            PageViewModels = new List<IPageViewModel>
            {
                new HomeViewModel(),
                new SearchViewModel()
             };

            _previousPages = new Stack<IPageViewModel>();
            _nextPages = new Stack<IPageViewModel>();

            CurrentPageViewModel = new HomeViewModel();
        }

        private void LogOutMethod(object parameter)
        {
            UserService.LogOut();
            OnUserChanged();
            PageViewModels = new List<IPageViewModel>
            {
                new HomeViewModel(),
                new SearchViewModel()
             };

            OnPropertyChanged(nameof(PageViewModels));
        }

        public void CloseMethod(object parameter)
        {
            (parameter as Window)?.Close();
        }

        public void LogInMethod(object parameter)
        {
            LogInViewModel logInViewModel = new LogInViewModel();
            new LogInWindow { DataContext = logInViewModel }.ShowDialog();

            if (UserLoggedIn && User.IsAdmin)
            {
                PageViewModels = PageViewModels.Append(new BookViewModel()).ToList();
                OnPropertyChanged(nameof(PageViewModels));
                PageViewModels = PageViewModels.Append(new AdminOrderViewModel()).ToList();
                OnPropertyChanged(nameof(PageViewModels));
            }
            else if (UserLoggedIn)
            {
                PageViewModels = PageViewModels.Append(new MyOrdersViewModel()).ToList();
                OnPropertyChanged(nameof(PageViewModels));
            }

            OnUserChanged();
        }

        public void RegisterMethod(object parameter)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            new RegisterWindow { DataContext = registerViewModel }.ShowDialog();
            OnUserChanged();
        }

        public bool CanExecuteAutorizationMethods(object parameter)
        {
            return Connected && !UserLoggedIn;
        }

        public bool CanExecuteLogOutMethod(object parameter)
        {
            return UserLoggedIn;
        }

        public void ChangePageMethod(object parameter)
        {
            if (parameter is IPageViewModel viewModel)
            {
                if (!PageViewModels.Contains(viewModel))
                {
                    PageViewModels.Add(viewModel);
                }

                if (parameter.GetType() != CurrentPageViewModel.GetType())
                {
                    _previousPages.Push(CurrentPageViewModel);

                    CurrentPageViewModel = viewModel;

                    viewModel.Update();
                }
            }
            else
            {
                throw new NotSupportedException($"Changing the page with parameter of type '{parameter.GetType().FullName}' is not supported");
            }
        }

        public void BackMethod(object parameter)
        {
            if (_previousPages.Count != 0)
            {
                if (parameter == null)
                {
                    _nextPages.Push(CurrentPageViewModel);
                }
                CurrentPageViewModel = _previousPages.Pop();
            }
        }
        public void ForwardMethod(object parameter)
        {
            if (_nextPages.Count != 0)
            {
                _previousPages.Push(CurrentPageViewModel);
                CurrentPageViewModel = _nextPages.Pop();
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public string GetName()
        {
            return User.Login;
        }

        private bool Connect()
        {
            var anonymousChannelFactory = new ChannelFactory<IAnonymousService>("IronMacbeth.BFF.AnonymousEndpoint");

            var anonymousServiceClient = anonymousChannelFactory.CreateChannel();

            AnonymousServerAdapter.ClearInstance();

            AnonymousServerAdapter.Initialize(anonymousServiceClient);

            try
            {
                return AnonymousServerAdapter.Instance.Ping();
            }
            catch
            {
                return false;
            }
        }

        private async void ConnectAsync()
        {
            ConnectionMenuText = "Connecting to server...";

            ConnectionMenuShown = true;
            ReconnectButtonShown = false;

            Connected = await Task<bool>.Factory.StartNew(Connect);
        }

        private async void ConnectionCheck(object sender, ElapsedEventArgs args)
        {
            if (checkTask.Status != TaskStatus.Running && Connected)
            {
                checkTask = new Task<bool>(() =>
                {
                    try
                    {
                        return AnonymousServerAdapter.Instance.Ping();
                    }
                    catch
                    {
                        return false;
                    }
                });

                checkTask.Start();

                Connected = await checkTask;
            }
        }

        private void ReconnectMethod(object parameter)
        {
            ConnectAsync();
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(AdminToolsVisibility));
            OnPropertyChanged(nameof(MenuVisibility));
            OnPropertyChanged(nameof(Login));

            // Forces view to re-check all CanExecute statuses
            CommandManager.InvalidateRequerySuggested();
        }
    }
}