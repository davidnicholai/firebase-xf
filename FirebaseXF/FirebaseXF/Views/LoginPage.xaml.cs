﻿using Xamarin.Forms;

namespace FirebaseXF
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _vm;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _vm = new LoginViewModel();
        }
    }
}