using Xamarin.Forms;

namespace FirebaseXF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginDemoPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
