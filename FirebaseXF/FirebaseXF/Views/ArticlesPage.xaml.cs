using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirebaseXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticlesPage : ContentPage
    {
        private ArticlesViewModel _vm;

        public ArticlesPage()
        {
            InitializeComponent();
            BindingContext = _vm = new ArticlesViewModel();
        }
    }
}