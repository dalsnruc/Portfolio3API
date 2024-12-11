using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IGenreDataService
    {

        IEnumerable<Genre> GetAllGenres();
        Genre GetGenreById(int genre_id);
        IEnumerable<Genre> GetGenresForTitle(string title_id);
        IEnumerable<Title> GetTitlesByGenre(int genre_id);


    }
}
