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

                var firebaseClient = new FirebaseClient(Constants.FirebaseBaseUrl);
                //var items = await firebaseClient.Child("users").WithAuth(TokenManager.Instance.Token).LimitToFirst(2)
                //    .OnceAsync<string>();

                //foreach (var item in items)
                //{
                //    System.Diagnostics.Debug.WriteLine($"GOTCHA {item}");
                //}
                // add new item to list of data 
                var item = await firebaseClient
                    .Child("fir-playground-13c13")
                    .WithAuth(TokenManager.Instance.Token) // <-- Add Auth token if required. Auth instructions further down in readme.
                    .PostAsync(new User());
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
