using Xamarin.Forms;

namespace FirebaseXF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new FirebaseXF.MainPage());
        }

        protected override void OnStart()
        {
            // Since the key does not exist yet, it means that this is the app's first run and the user is not logged in.
            if (!Current.Properties.ContainsKey(Constants.PropKeyIsLoggedIn))
            {
                navigateToLoginPage();
            }

            if (Current.Properties.ContainsKey(Constants.PropKeyIsLoggedIn))
            {
                bool? isLoggedIn = Current.Properties[Constants.PropKeyIsLoggedIn] as bool?;
                if (isLoggedIn != null && !((bool)isLoggedIn))
                {
                    navigateToLoginPage();
                }
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async void navigateToLoginPage()
        {
            await ((NavigationPage)MainPage).PushAsync(new LoginPage(), true);
        }
    }
}
