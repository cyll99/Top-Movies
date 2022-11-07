using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace Top_Movies
{/// <summary>
/// Nom : LAROSE
/// Prenom : Christ-Yan Love
/// </summary>
    public class Film
    {
        private string adult { get; set; }
        public String backdrop_path { get; set; }

        private List<int> _genre_ids;

        public List<int> genre_ids()
        {
            return _genre_ids;
        }

        public void Setgenre_ids(List<int> value)
        {
            _genre_ids = value;
        }

        public int id { get; set; }
        public String original_title { get; set; }
        public String overview { get; set; }
        public String poster_path { get; set; }
        public String release_date { get; set; }
        public String original_language { get; set; }
        public float popularity { get; set; }
        public String title { get; set; }
        public bool video { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
        public byte[] byte_post { get; set; }
        public byte[] byte_back { get; set; }
    }

    public class Utilities
    {
        private static string siteLink;
        public static List<Film> getMovieDbList(/*String address*/)
        {
            String reponse = "";
            List<Film> Films = null;
            siteLink = "https://api.themoviedb.org/3/movie/now_playing?api_key=a07e22bc18f5cb106bfe4cc1f83ad8ed";
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    reponse = webClient.DownloadString(siteLink);
                }
                //Console.WriteLine(retVal);
                using (JsonDocument document = JsonDocument.Parse(reponse))
                {
                    JsonElement root = document.RootElement;
                    JsonElement resultsList = root.GetProperty("results");
                    Films = JsonSerializer.Deserialize<List<Film>>(resultsList);
                    foreach (Film film in Films)
                        Debug.WriteLine(film);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Films;
        }

        public static bool IsConnectedToInternet()
        {
            string host = "www.google.com";
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
    }

}
