using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirebaseXF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginDemoPage : ContentPage
    {
        private LoginDemoViewModel _vm;

        public LoginDemoPage()
        {
            InitializeComponent();
            BindingContext = _vm = new LoginDemoViewModel();
        }
    }
}