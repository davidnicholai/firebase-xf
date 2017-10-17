using System;
using System.Net.Http;
using System.Windows.Input;
using Firebase.Xamarin.Auth;
using Xamarin.Auth;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace FirebaseXF
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _log;
        private string _jwt = string.Empty;
        private ICommand _loginCommand;
        private ICommand _registerCommand;

        public Action PopPageAction { get; set; }

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

        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
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

        public LoginViewModel()
        {
            LoginCommand = new Command(LoginAsync);
            RegisterCommand = new Command(RegisterAsync);
        }

        private async void LoginAsync()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                TokenManager.Instance.Token = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;

                StoreCredentials(Email, Password);

                PopPageAction?.Invoke();
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HttpRequestException {e.Message}");
                Log = "HttpRequestException " + e.Message;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e.Message}");
                Log = "Exception " + e.Message;
            }
        }

        private async void RegisterAsync()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));

            try
            {
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                TokenManager.Instance.Token = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;

                StoreCredentials(Email, Password);

                PopPageAction?.Invoke();
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HttpRequestException {e.Message}");
                Log = "HttpRequestException" + e.Message;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e.Message}");
                Log = "Exception";
            }
        }

        private void StoreCredentials(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                Account account = new Account()
                {
                    Username = email,
                };
                account.Properties.Add("Password", password);
                AccountStore.Create().Save(account, Constants.AppName);
            }
        }

    }
}
