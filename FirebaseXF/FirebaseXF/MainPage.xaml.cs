using Xamarin.Forms;

namespace FirebaseXF
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _vm;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _vm = new MainViewModel();
        }
    }
}
