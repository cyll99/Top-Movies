using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.IO;

namespace Top_Movies
{
    public class MovieDatabase
    {
        public static SQLiteConnection Connection()
        {
            String libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(libraryPath, "film.db3");

            SQLiteConnection connection = new SQLiteConnection(path);
            connection.CreateTable<Film>();

            return connection;
        }

        public static void SaveFilm(Film film)
        {
            if (!GetFilmID().Contains(film.id))
            {
                Connection().Insert(film);
            }
        }

        public static List<Film> GetFilms()
        {
            List<Film> listMovie = new List<Film>();

            var films = Connection().Table<Film>();

            foreach (Film film in films)
            {
                listMovie.Add(film);
            }
            return listMovie;
        }


        public static List<int> GetFilmID()
        {
            List<int> listMovie = new List<int>();

            var films = Connection().Table<Film>();

            foreach (Film film in films)
            {
                listMovie.Add(film.id);
            }
            return listMovie;
        }

    }
}
