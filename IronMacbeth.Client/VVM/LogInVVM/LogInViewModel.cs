using System.Windows;
using System.Windows.Input;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.LogInVVM
{
    public class LogInViewModel
    {
        private bool _loginMode;

        public string ButtonContent => _loginMode ? "Log in" : "Register";

        public string Login { get; set; }
        public string Password { private get; set; }

        public User User { get; private set; }

        public ICommand LogInCommand { get; }
        public ICommand CloseCommand { get; }

        public LogInViewModel(bool loginMode = true)
        {
            _loginMode = loginMode;
            LogInCommand = new RelayCommand(LogInMethod) {CanExecuteFunc = LogInCanExecute};
            CloseCommand = new RelayCommand(CloseMethod);
        }

        public void LogInMethod(object parameter)
        {
            if (_loginMode)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    User = MainViewModel.Proxy.LogIn(Login, "");
                }
                else
                {
                    User = MainViewModel.Proxy.LogIn(Login, Password);
                }
                if (User != null)
                {
                    CloseMethod(parameter);
                }
                else
                {
                    MessageBox.Show("Incorrect username or password", "error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                User user = new User
                {
                    Login = Login,
                    Password = Password
                };

                MainViewModel.ServerAdapter.Register(user);

                _loginMode = true;

                LogInMethod(parameter);
            }
        }

        public bool LogInCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Login);
        }

        public void CloseMethod(object parameter)
        {
            (parameter as Window)?.Close();
        }
    }
}