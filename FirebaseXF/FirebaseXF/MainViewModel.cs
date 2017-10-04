using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Firebase.Xamarin.Auth;
using FirebaseXF.Annotations;
using Xamarin.Forms;


namespace FirebaseXF
{
    public class MainViewModel : INotifyPropertyChanged
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

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged(nameof(Log));
            }
        }

        public ICommand LoginCommand
        {
            get => _loginCommand;
            set { _loginCommand = value; OnPropertyChanged(nameof(LoginCommand)); }
        }

        public ICommand RegisterCommand
        {
            get => _registerCommand;
            set { _registerCommand = value; OnPropertyChanged(nameof(RegisterCommand)); }
        }

        public ICommand FacebookLoginCommand
        {
            get => _facebookLoginCommand;
            set { _facebookLoginCommand = value; OnPropertyChanged(nameof(FacebookLoginCommand)); }
        }


        public MainViewModel()
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
                Log = "HttpRequestException"  + e.Message;
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

        #region INotifyPropertyChanged Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
} 