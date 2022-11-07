using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Top_Movies
{
    class SqliteDataAccess
    {

        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location; // path of the exe file
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath); // working directory
        static string dataFilePath = System.IO.Path.Combine(strWorkPath, "films.db"); // path database file

        static string conn = $"Data Source={dataFilePath};Version=3"; // connection string
        public static void CreateIfNotExists()
        {
            using (IDbConnection cnn = new SQLiteConnection(conn))
            {

                var query = "CREATE TABLE IF NOT EXISTS Films ( id INTEGER, title TEXT, poster BLOB, backdrop BLOB, release_date TEXT, original_language TEXT,  vote_count INTEGER, vote_average REAL, popularity REAL, overview	TEXT)";
              
                cnn.Execute(query, new DynamicParameters());
                if (Utilities.IsConnectedToInternet())
                {
                    List<Film> listFilm = Utilities.getMovieDbList();
                    foreach (Film film in listFilm)
                        SaveFilm(film);
                }
              
            }
        }
        /// <summary>
        /// Load film from local database
        /// </summary>
        /// <returns> List of films</returns>
        public static List<Film> LoadFilms()
        {
            List<Film> films = new List<Film>();
            using (SQLiteConnection cnn = new SQLiteConnection(conn))
            {
                var query = "select * from Films";
                cnn.Open();

                SQLiteCommand sQLiteCommand = new SQLiteCommand(query, cnn);
                SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
                if (sQLiteDataReader.HasRows)
                {
                    
                    while (sQLiteDataReader.Read())
                    {
                        Film film = new Film();
                        film.title = (string)sQLiteDataReader["title"];
                        film.release_date = (string)sQLiteDataReader["release_date"];
                        film.original_language = (string)sQLiteDataReader["original_language"];
                        film.vote_count = Convert.ToInt32(sQLiteDataReader["vote_count"]);
                        film.overview = (string)sQLiteDataReader["overview"];
                        film.popularity = Convert.ToInt32(sQLiteDataReader["popularity"]);
                        film.vote_average = Convert.ToInt32(sQLiteDataReader["vote_average"]);
                        byte[] byte_poster = (byte[])sQLiteDataReader["poster"];
                        byte[] byte_back = (byte[])sQLiteDataReader["backdrop"];


                        film.byte_post = byte_poster;
                        film.byte_back = byte_back;

                        films.Add(film);
                    }
                }

                return films;
            }

        }
        
        /// <summary>
        /// Insert film in local database
        /// </summary>
        /// <param name="film"></param>
        public static void SaveFilm(Film film)
        {
            using (SQLiteConnection cnn = new SQLiteConnection(conn))
            {
                cnn.Open();


                var backdrop = "https://image.tmdb.org/t/p/w342" + film.backdrop_path;
                var post = "https://image.tmdb.org/t/p/w342" + film.poster_path;

                //byte[] picBac = ImageToByte(backdrop, System.Drawing.Imaging.ImageFormat.Jpeg);
                //byte[] picPos = ImageToByte(post, System.Drawing.Imaging.ImageFormat.Jpeg);

                string sql = @"
                        insert into Films (title, overview, poster, backdrop, release_date, vote_count,id,original_language,vote_average,popularity)
                        Select @title , @overview, @poster, @backdrop, @release_date,@vote_count, @id, @original_language,@vote_average,@popularity
                        Where not exists (
                            select * 
                            from Films 
                            where 
                                id = @id
                          
)
                        ";
                using ( var cmd = new SQLiteCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@title", film.title);
                    cmd.Parameters.AddWithValue("@overview", film.overview);
                    //cmd.Parameters.AddWithValue("@poster", picPos);
                    //cmd.Parameters.AddWithValue("@backdrop", picBac);
                    cmd.Parameters.AddWithValue("@release_date", film.release_date);
                    cmd.Parameters.AddWithValue("@vote_count", film.vote_count);
                    cmd.Parameters.AddWithValue("@id", film.id);
                    cmd.Parameters.AddWithValue("@original_language", film.original_language);
                    cmd.Parameters.AddWithValue("@vote_average", film.vote_average);
                    cmd.Parameters.AddWithValue("@popularity", film.popularity);
                    cmd.ExecuteNonQuery();
         
                }


            }
        }


        /// <summary>
        /// convert image from uri to byte
        /// </summary>
        /// <param name="backdrop"></param>
        /// <param name="format"></param>
        /// <returns>a list of byte</returns>
        //public static byte[] ImageToByte(string backdrop, System.Drawing.Imaging.ImageFormat format)
        //{
        //    WebClient client = new WebClient();
        //    Stream stream = client.OpenRead(backdrop);
        //    // Bitmap bitmap; 
        //    Image bitmap = new Bitmap(Image.FromStream(stream));

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        // Convert Image to byte[]
        //        bitmap.Save(ms, format);
        //        byte[] imageBytes = ms.ToArray();
        //        return imageBytes;
        //    }
          
        //}

        /// <summary>
        /// convert byte to image
        /// </summary>
        /// <param name="bytesArr"></param>
        /// <returns>an image</returns>

        //public static Image byteArrayToImage(byte[] bytesArr)
        //{
        //    using (MemoryStream memstr = new MemoryStream(bytesArr))
        //    {
        //        Image img = Image.FromStream(memstr);
        //        return img;
        //    }
        //}

        /// <summary>
        /// change the color of the icon if there is internet
        /// </summary>
        //public static void changeColor(System.Web.UI.WebControls.Image  Image1)
        //{
        //    if (Utilities.IsConnectedToInternet())
        //        Image1.ImageUrl = "~/ressources/blue.png";
        //    else
        //        Image1.ImageUrl = "~/ressources/red.png";
        //}

    }


}
