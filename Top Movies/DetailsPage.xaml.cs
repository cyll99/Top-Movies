using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Top_Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        private Film currentFilm;
        public const String VIDEO_URL = "https://api.themoviedb.org/3/movie/{0}/videos?api_key=a07e22bc18f5cb106bfe4cc1f83ad8ed";
        public DetailsPage(Film film)
        {
            InitializeComponent();
            currentFilm = film;
            Display();
        }

        private void Display()
        {
            try
            {
                lblTitle.Text = currentFilm.title;
                lblOverView.Text = currentFilm.overview;
                lblDate.Text = "Date: " + currentFilm.release_date;
                lblLanguage.Text = "Language: " + currentFilm.original_language;
                lblPopularity.Text = "Popularity: " + currentFilm.popularity.ToString();
                lblVoteCount.Text = "Vote Count: " + currentFilm.vote_count.ToString();
                lblVoteAverage.Text = "Vote Average: " + currentFilm.vote_average.ToString();

                string circleImgUrl = "";
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    circleImgUrl = "circleBlue.png";
                    displayVideo(Utilities.getYoutubeKey(VIDEO_URL, currentFilm), circleImgUrl);
                }
                else
                {
                    circleImgUrl = "circleRed.png";
                    displayVideo("", circleImgUrl);
                }
            }
            catch (Exception) { }


        }

        private void displayVideo(String id, String circleImgUrl)
        {
            videoView.Source = "https://www.youtube.com/watch?v=" + id;
            imgCircle.Source = circleImgUrl;
        }

    }
}
