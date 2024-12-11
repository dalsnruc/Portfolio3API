

namespace DataLayer
{
    public class GenreDataService : IGenreDataService
    {

        public IEnumerable<Genre> GetAllGenres()
        {
            using var db = new imdbContext();
            return db.Genres.ToList();
        }

        public Genre GetGenreById(int genre_id)
        {
            using var db = new imdbContext();
            return db.Genres
                .FirstOrDefault(g => g.Id == genre_id);
        }

        public IEnumerable<Genre> GetGenresForTitle(string title_id)
        {
            using var db = new imdbContext();
            return db.TitleGenres
                .Where(tg => tg.TitleId == title_id)
                .Select(tg => tg.Genre)
                .ToList();
        }

        public IEnumerable<Title> GetTitlesByGenre(int genre_id)
        {
            using var db = new imdbContext();
            // Join Title_genres with TitleBasics and filter by genre_id
            return db.TitleGenres
                .Where(tg => tg.GenreId == genre_id)
                .Select(tg => tg.Title)
                .ToList();
        }

    }
}
