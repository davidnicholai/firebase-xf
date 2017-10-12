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
        private const string FirebaseBaseUrl = "";
        private const string FirebaseApiKey = "";

        private const string FacebookAppId = "";
        private const string FacebookAppSecret = "";

        private string _email;
        private string _password;
        private string _log;
        private string _jwt = string.Empty;
        private ICommand _loginCommand;
        private ICommand _registerCommand;
        private ICommand _facebookLoginCommand;

        public Action PopAction { get; set; }

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

        public ICommand FacebookLoginCommand
        {
            get { return _facebookLoginCommand; }
            set { SetProperty(ref _facebookLoginCommand, value); }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(AsyncLogin);
            RegisterCommand = new Command(AsyncRegister);
            FacebookLoginCommand = new Command(AsyncFacebookLogin);
        }

        private async void AsyncLogin()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));

            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                _jwt = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;
                Log = _jwt;

                TokenManager.Instance.Token = _jwt;

                storeCredentials(Email, Password);
                Application.Current.Properties.Add(Constants.PropKeyIsLoggedIn, true);

                PopAction?.Invoke();
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

        private async void AsyncRegister()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));

            try
            {
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                _jwt = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;
                Log = _jwt;

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

        private async void AsyncFacebookLogin()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
            const string facebookAccessToken = FacebookAppSecret;

            try
            {
                var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, "");

                _jwt = string.IsNullOrEmpty(auth.FirebaseToken) ? string.Empty : auth.FirebaseToken;
                Log = _jwt;
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
