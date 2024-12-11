using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : Controller
    {
        private readonly IGenreDataService _genreService;

        public GenreController(IGenreDataService genreService)
        {
            _genreService = genreService;
        }

        //Retrieve a list of all genres
        [HttpGet]
        public IActionResult GetAllGenres()
        {
            var genres = _genreService.GetAllGenres();
            return Ok(genres);
        }

        //Get a Genre by id
        [HttpGet("{id}")]
        public IActionResult GetGenreById(int id)
        {
            var genre = _genreService.GetGenreById(id);
            if (genre == null)
            {
                return NotFound("Genre not found");
            }
            else
            {
                return Ok(genre);
            }

        }

        //Get all the genres connected to a Title
        [HttpGet("title/{title_id}")]
        public IActionResult GetGenresForTitle(string title_id)
        {
            var genres = _genreService.GetGenresForTitle(title_id);
            return Ok(genres);
        }


        //Get all Titles that has a specific genre connected to them
        [HttpGet("titles/{genre_id}")]
        public IActionResult GetTitlesByGenre(int genre_id)
        {
            var titles = _genreService.GetTitlesByGenre(genre_id);
            if (titles == null || !titles.Any())
            {
                return NotFound("No titles found for this genre.");
            }
            return Ok(titles);
        }
    }
}
