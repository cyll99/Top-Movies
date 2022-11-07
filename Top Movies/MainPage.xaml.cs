using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Top_Movies
{
    public partial class MainPage : ContentPage
    {
        public static int index = 0;
        public static List<Film> listFilm;
        Film currentFilm;
        public MainPage()
        {
            SqliteDataAccess.CreateIfNotExists();// create the database
            //SqliteDataAccess.changeColor(Image1); // change the icon
            // save the films in the datavase
            if (Utilities.IsConnectedToInternet())
            {
                listFilm = Utilities.getMovieDbList();
                foreach (Film film in listFilm)
                    SqliteDataAccess.SaveFilm(film);
            }
            else
            {
                listFilm = SqliteDataAccess.LoadFilms();//load film from database
            }

            afficher(index);

        }

        /// <summary>
        /// display film from website or database
        /// </summary>
        /// <param name="index"></param>
        public void afficher(int index)
        {


            if (Utilities.IsConnectedToInternet())
            {// films from api
                //listFilm = Utilities.getMovieDbList();
                currentFilm = listFilm.ElementAt(index);
                //SqliteDataAccess.SaveFilm(film);

                lbl_title.Text = currentFilm.title;
                lbl_overview.Text = currentFilm.overview;
                Iposter.Source = "https://image.tmdb.org/t/p/w342" + currentFilm.poster_path;
            }
            else
            {// films from database
                listFilm = SqliteDataAccess.LoadFilms();
                currentFilm = listFilm.ElementAt(index);
                lbl_title.Text = currentFilm.title;
                lbl_overview.Text = currentFilm.overview;
                Iposter.Source = "data:image/jpeg;base64," + Convert.ToBase64String(currentFilm.byte_post);
            }

            if (index > 0)
                btn_prece.IsEnabled = true;
            else
                btn_prece.IsEnabled = false;


            if (index == 19)
                btn_suiv.IsEnabled = false;
            else
                btn_suiv.IsEnabled = true;

            

        }

        private void btn_prece_Clicked(object sender, EventArgs e)
        {
            afficher(--index);
        }

        private void btn_suiv_Clicked(object sender, EventArgs e)
        {
            afficher(++index);
        }

        private async void Iposter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DetailPage());
        }
    }
}
