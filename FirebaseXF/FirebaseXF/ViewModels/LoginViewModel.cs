using System;
using System.Net.Http;
using System.Windows.Input;
using Firebase.Xamarin.Auth;
using Xamarin.Auth;
using Xamarin.Forms;
using System.Linq;

namespace FirebaseXF
{
    public class LoginViewModel : BaseViewModel
    {
        #region Fields and Properties

        private string _email;
        private string _password;
        private ICommand _loginCommand;
        private ICommand _registerCommand;

        private string _log;

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

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new Command(LoginAsync);
            RegisterCommand = new Command(RegisterAsync);
            Log = "Status: Nothing going on yet, but make sure to register a user via this app if you haven't already.";
        }

        #region Login and Register

        /// <summary>
        /// Login implementation. Make sure to populate YOUR_API_KEY!
        /// </summary>
        private async void LoginAsync()
        {
            try
            {
                // TODO: Replace YOUR_API_KEY to an actual API key from Firebase.
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCY6jbDmUBmDAVD40OnZwada-cXq4CzpJg"));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                Log = "Successfully logged in! Your token is: " + auth.FirebaseToken;
                // From here, you have a token (in auth.FirebaseToken) which you can use to
                // do any of Firebase's features, make sure to store it somewhere!
            }
            catch (HttpRequestException e)
            {
                Log = "An HttpRequestException occurred.";
            }
            catch (Exception e)
            {

                Log = "An Exception occurred.";
            }
        }

        /// <summary>
        /// Register implementation. Make sure to populate YOUR_API_KEY!
        /// </summary>
        private async void RegisterAsync()
        {
            try
            {
                // TODO: Replace YOUR_API_KEY to an actual API key from Firebase.
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCY6jbDmUBmDAVD40OnZwada-cXq4CzpJg"));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                Log = "Successfully registered! Your token is: " + auth.FirebaseToken;
                // From here, you have a token (in auth.FirebaseToken) which you can use to
                // do any of Firebase's features, make sure to store it somewhere!
            }
            catch (HttpRequestException e)
            {
                Log = "An HttpRequestException occurred.";
            }
            catch (Exception e)
            {
                Log = "An Exception occurred.";
            }
        }

        #endregion

        #region Securely handling credentials

        /// <summary>
        /// Stores your user's credentials in a secure manner. It's up to you where you will include this method.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private void StoreCredentials(string email, string password)
        {
            Xamarin.Auth.Account account = new Account()
            {
                Username = email
            };
            account.Properties.Add("Password", password);
            Xamarin.Auth.AccountStore.Create().Save(account, "FirebaseXF");

        }

        /// <summary>
        /// Retrieves your user's credentials which was stored in previous sessions.
        /// </summary>
        /// <returns></returns>
        private Xamarin.Auth.Account GetCredentials()
        {
            return AccountStore.Create().FindAccountsForService("FirebaseXF").FirstOrDefault();
        }

        /// <summary>
        /// Checks if the device contains any credentials.
        /// </summary>
        /// <returns></returns>
        private bool HasStoredCredentials()
        {
            if (GetCredentials() != null)
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}
