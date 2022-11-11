using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Top_Movies
{
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

                String circleImgUrl = "";
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    img.Source = "https://image.tmdb.org/t/p/w342" + currentFilm.backdrop_path;
                    circleImgUrl = "circleBlue.png";
                }
                else
                {
                    circleImgUrl = "circleRed.png";
                    try
                    {
                        //var stream1 = new MemoryStream(currentFilm.image);
                       var source = ImageSource.FromStream(() => new MemoryStream(currentFilm.image));
                     
                        img.Source = source;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Bad Connection Internet");
                    }
                }

                imgCircle.Source = circleImgUrl;
            }
            catch (Exception) { }

        }
    }
}