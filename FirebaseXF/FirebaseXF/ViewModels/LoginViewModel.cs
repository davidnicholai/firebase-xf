using System;
using System.Net.Http;
using System.Windows.Input;
using Firebase.Xamarin.Auth;
using Xamarin.Auth;
using Xamarin.Forms;

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
            LoginCommand = new Command(AsyncLogin);
            RegisterCommand = new Command(AsyncRegister);
        }

        private async void AsyncLogin()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                TokenManager.Instance.Token = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;

                storeCredentials(Email, Password);

                //Application.Current.Properties.Add(Constants.PropKeyIsLoggedIn, true);

                PopPageAction?.Invoke();
            }
            catch (HttpRequestException e)
            {
                Log = "HttpRequestException " + e.Message;
            }
            catch (Exception e)
            {
                Log = "Exception " + e.Message;
            }
        }

        private async void AsyncRegister()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));

            try
            {
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                TokenManager.Instance.Token = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;

                storeCredentials(Email, Password);
                Application.Current.Properties.Add(Constants.PropKeyIsLoggedIn, true);

                PopPageAction?.Invoke();
            }
            catch (HttpRequestException e)
            {
                Log = "HttpRequestException" + e.Message;
            }
            catch (Exception e)
            {
                Log = "Exception";
            }
        }

        private void storeCredentials(string email, string password)
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
