using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FirebaseXF
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand _navigateArticlesCommand;

        public ICommand NavigateArticlesCommand
        {
            get { return _navigateArticlesCommand; }
            set { SetProperty(ref _navigateArticlesCommand, value); }
        }

        public Action NavigateArticlesAction { get; set; } 

        public MainViewModel()
        {
            NavigateArticlesCommand = new Command(navigateToArticlesPage);
        }

        private void navigateToArticlesPage()
        {
            NavigateArticlesAction?.Invoke();
        }
    }
} 