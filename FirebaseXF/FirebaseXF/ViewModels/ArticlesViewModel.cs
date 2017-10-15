using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Input;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace FirebaseXF.ViewModels
{
    public class ArticlesViewModel : BaseViewModel
    {
        private ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get { return _refreshCommand; }
            set { SetProperty(ref _refreshCommand, value); }
        }

        public IEnumerable<string> Names { get; set; }

        public ArticlesViewModel()
        {
            RefreshCommand = new Command(refreshList);
        }

        private async void refreshList()
        {
            try
            {
                var firebaseClient = new FirebaseClient(Constants.FirebaseDbUrl);
                var items = await firebaseClient
                    .Child("our-users")
                    .WithAuth(TokenManager.Instance.Token)
                    .OnceAsync<User>();

                foreach (var item in items)
                {
                    System.Diagnostics.Debug.WriteLine($"GOTCHA {item.Object.FirstName}");
                }
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HttpRequestException {e.Message}");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e.Message}");
            }
        }

        private async void saveData()
        {
            try
            {
                var firebase = new FirebaseClient(Constants.FirebaseDbUrl);

                var token = TokenManager.Instance.Token;

                var item = await firebase
                    .Child("our-users")
                    .WithAuth(TokenManager.Instance.Token)
                    .PostAsync(new User() { FirstName = "John", LastName = "Do2eth" }, false);

                System.Diagnostics.Debug.WriteLine($"Key for the new item: {item.Key}");
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HttpRequestException {e.Message}");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e.Message}");
            }
        }
    }
}
