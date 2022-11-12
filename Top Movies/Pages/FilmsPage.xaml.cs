using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Top_Movies
{
    /// <summary>
    /// Christ-Yan Love Larose
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilmsPage : ContentPage
    {
        private int index = 0;
        private List<Film> films;
        private Film currentFilm;
        public FilmsPage(List<Film> listeFilms)
        {
            InitializeComponent();
            films = listeFilms;
            display(index);
        }

        private void btnPrevious_Clicked(object sender, EventArgs e)
        {
            display(--index);
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            display(++index);
        }

        private async void img_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DetailsPage(currentFilm));
        }

        /// <summary>
        /// Method to bind data with the view 
        /// </summary>
        /// <param name="index"></param>
        private void display(int index)
        {
            try
            {
                if (index == 0)
                {
                    btnPrevious.IsEnabled = false;
                }
                else if (index == films.Count - 1)
                {
                    btnNext.IsEnabled = false;
                }
                else
                {
                    btnNext.IsEnabled = true;
                    btnPrevious.IsEnabled = true;
                }

                currentFilm = films.ElementAt(index);


                lblTitle.Text = currentFilm.title;
                lblOverview.Text = currentFilm.overview;

               
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    imgCircle.BackgroundColor = Color.Blue;

                    img.Source = "https://image.tmdb.org/t/p/w342" + currentFilm.backdrop_path;

                   
                }
                else
                {
                    imgCircle.BackgroundColor = Color.Red;
                    try
                    {
                        img.Source = "https://image.tmdb.org/t/p/w342" + currentFilm.backdrop_path;

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Bad Connection Internet");
                    }
                }

            }
            catch (Exception) { }

        }
    }
}