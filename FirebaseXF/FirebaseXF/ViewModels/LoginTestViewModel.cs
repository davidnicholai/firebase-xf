using System;
using System.Net.Http;
using System.Windows.Input;
using Firebase.Xamarin.Auth;
using Xamarin.Forms;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Auth;
using System.Linq;

namespace FirebaseXF
{
    public class LoginTestViewModel : BaseViewModel
    {
        #region Fields
        private string _email;
        private string _password;
        private string _name;
        private string _log;
        private string _token = string.Empty;
        private ICommand _loginCommand;
        private ICommand _registerCommand;
        private ICommand _sendCommand;
        private ICommand _fetchCommand;
        #endregion

        #region Properties
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

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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

        public ICommand SendCommand
        {
            get { return _sendCommand; }
            set { SetProperty(ref _sendCommand, value); }
        }

        public ICommand FetchCommand
        {
            get { return _fetchCommand; }
            set { SetProperty(ref _fetchCommand, value); }
        }
        #endregion

        public LoginTestViewModel()
        {
            LoginCommand = new Command(LoginAsync);
            RegisterCommand = new Command(RegisterAsync);
            SendCommand = new Command(SendDataAsync);
            FetchCommand = new Command(ReadDataAsync);

            var isStored = HasStoredCredentials();
        }

        private async void LoginAsync()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                _token = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;

                StoreCredentials(Email, Password);

                Log = "Successfully logged in!";
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
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                _token = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;

                StoreCredentials(Email, Password);

                Log = "Successfully registered!";
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

        #region Firebase Database
        private async void SendDataAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                Log = "User is not logged in!";
                return;
            }
            else if (string.IsNullOrEmpty(Name))
            {
                Log = "Field is empty!";
                return;
            }

            try
            {
                var firebase = new FirebaseClient(Constants.FirebaseDbUrl);

                var token = TokenManager.Instance.Token;

                var item = await firebase
                    .Child("names")
                    .WithAuth(_token)
                    .PostAsync(Name, false);

                Log = $"Sent {Name} to the server";
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HttpRequestException {e.Message}");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e.Message}");
            }
        }

        private async void ReadDataAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                Log = "User is not logged in!";
                return;
            }

            try
            {
                var firebaseClient = new FirebaseClient(Constants.FirebaseDbUrl);
                var items = await firebaseClient
                    .Child("names")
                    .WithAuth(_token)
                    .OnceAsync<string>();

                Log = string.Empty;

                foreach (var item in items)
                {
                    Log += item.Object + " ";
                    System.Diagnostics.Debug.WriteLine($"GOTCHA {item.Object}");
                }
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HttpRequestException {e.Message}");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e.Message}");
            }
        }
        #endregion

        private void StoreCredentials(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                Account account = new Account()
                {
                    Username = email
                };
                account.Properties.Add("Password", password);
                AccountStore.Create().Save(account, Constants.AppName);
            }
        }

        private Account GetCredentials()
        {
            return AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
        }

        private bool HasStoredCredentials()
        {
            var credentials = GetCredentials();
            if (credentials != null)
            {
                return true;
            }

            return false;
        }
    }
}
