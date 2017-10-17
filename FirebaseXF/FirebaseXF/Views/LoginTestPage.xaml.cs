using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirebaseXF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginTestPage : ContentPage
    {
        private LoginTestViewModel _vm;

        public LoginTestPage()
        {
            InitializeComponent();
            BindingContext = _vm = new LoginTestViewModel();
        }
    }
}