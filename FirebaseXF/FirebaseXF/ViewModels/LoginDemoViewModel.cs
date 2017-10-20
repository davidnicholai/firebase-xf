using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Forms;

namespace FirebaseXF
{
    public class LoginDemoViewModel : BaseViewModel
    {
        #region Fields and Properties

        private string _email;
        private string _password;
        private ICommand _loginCommand;
        private ICommand _registerCommand;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public ICommand LoginCommand
        {
            get { return _loginCommand; }
            set { SetProperty(ref _loginCommand, value); }
        }

        public ICommand RegisterCommand
        {
            get { return _registerCommand; }
            set { SetProperty(ref _registerCommand, value); }
        }

        #endregion

        public LoginDemoViewModel()
        {
            LoginCommand = new Command(LoginAsync);
            RegisterCommand = new Command(RegisterAsync);
        }

        #region Login and Register

        private async void LoginAsync()
        {
            System.Diagnostics.Debug.WriteLine("Clicked!");
            // TODO: Implement this method.
        }

        private async void RegisterAsync()
        {
            System.Diagnostics.Debug.WriteLine("Clicked!");
            // TODO: Implement this method.
        }

        #endregion

        #region Securely handling credentials

        private void StoreCredentials()
        {
            // TODO: Implement this method.
        }

        private Xamarin.Auth.Account GetCredentials()
        {
            // TODO: Implement this method.
            return new Account();
        }

        #endregion
    }
}
