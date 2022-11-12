using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Top_Movies
{
    /// <summary>
    /// Christ-Yan Love Larose
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public SQLiteConnection conn;
        private Image splashImage;
        private List<Film> films;
        public MainPage()
        {
            InitializeComponent();

            GetListMovie();
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = "letterf",
                WidthRequest = 100,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutFlags(splashImage,
               AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage,
             new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splashImage);

            this.BackgroundColor = Color.FromHex("#2296f3");
            this.Content = sub;
        }

        private void GetListMovie()
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                try
                {
                    // Connection to internet is available
                    films = Utilities.getMovieDbList();
                    foreach (Film film in films)
                    {
                        MovieDatabase.SaveFilm(film);
                    }
                }
                catch (Exception) { }

            }
            else
            {
                try
                {
                    films = MovieDatabase.GetFilms();
                    
                }
                catch (Exception) { }
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                await splashImage.ScaleTo(1, 2000); //Time-consuming processes such as initialization
                await splashImage.ScaleTo(0.9, 1500, Easing.Linear);
                await splashImage.ScaleTo(150, 1200, Easing.Linear);
                Application.Current.MainPage = new NavigationPage(new FilmsPage(films));    //After loading  MainPage it gets Navigated to our FilmPage

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
