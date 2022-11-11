using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace Top_Movies
{
    public static class Utilities
    {
        private static string siteLink = "https://api.themoviedb.org/3/movie/now_playing?api_key=a07e22bc18f5cb106bfe4cc1f83ad8ed";


        /// <summary>
        /// Method to get Json file from The TMDB
        /// and return a list of movie
        /// </summary>
        /// <returns></returns>
        public static List<Film> getMovieDbList()
        {
            String reponse = "";
            List<Film> Films = null;
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    reponse = webClient.DownloadString(siteLink);
                }

                using (JsonDocument document = JsonDocument.Parse(reponse))
                {
                    JsonElement root = document.RootElement;
                    JsonElement resultsList = root.GetProperty("results");
                    Films = JsonSerializer.Deserialize<List<Film>>(resultsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Films;
        }


        public static String getYoutubeKey(string VIDEO_URL, Film currentFilm)
        {
            String reponse = "";
            String youtubeKey = "";

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    reponse = webClient.DownloadString(String.Format(VIDEO_URL, currentFilm.id));
                }

                using (JsonDocument document = JsonDocument.Parse(reponse))
                {
                    JsonElement root = document.RootElement;
                    JsonElement resultsList = root.GetProperty("results");

                    int i = 0;
                    while (true)
                    {
                        String type = resultsList[i].GetProperty("type").ToString();
                        youtubeKey = resultsList[i].GetProperty("key").ToString();
                        if (type.Equals("Clip"))
                        {
                            break;
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return youtubeKey;
        }


        /// <summary>
        /// This method check if the user is connected to internet
        /// </summary>
        /// <returns></returns>
        public static bool isConnectedToInternet()
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


        /// <summary>
        /// This method convert the image to byte
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }


        /// <summary>
        /// This method convert the bytes to Base64ToImage(string base64String)
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static Image ByteToImage(byte[] imageBytes)
        {
            //Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }

    }
}
