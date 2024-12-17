using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.NameModels;
using WebApi.Models.TitleModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/titles")]

public class TitleController : BaseController
{
    ITitleDataService _titledataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public TitleController(
        ITitleDataService titleDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _titledataservice = titleDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;

    }

    [HttpGet("movies", Name = nameof(GetTitlesPaged))]
    public IActionResult GetTitlesPaged(int page = 0, int pageSize = 20, string? genre = null, double? minRating = null, string? primaryTitle = null)
    {

        var titles = _titledataservice
            .GetTitles(page, pageSize, genre, minRating, primaryTitle)
            .Select(CreateAllTitlesModel);

        var numberOfItems = _titledataservice.NumberOfTitles();
        object result = CreatePaging(
            nameof(GetTitlesPaged),
            page,
            pageSize,
            numberOfItems,
            titles);
        return Ok(result);


    }

    [HttpGet("tvseries", Name = nameof(GetTvSeriesPaged))]
    public IActionResult GetTvSeriesPaged(int page = 0, int pageSize = 20, string? genre = null, double? minRating = null, string? primaryTitle = null)
    {
        var titles = _titledataservice
            .GetTvSeries(page, pageSize, genre, minRating, primaryTitle)
            .Select(CreateAllTitlesModel);

        var numberOfItems = _titledataservice.NumberOfTitles();
        object result = CreatePaging(
            nameof(GetTvSeriesPaged),
            page,
            pageSize,
            numberOfItems,
            titles);

        return Ok(result);
    }




    [HttpGet("top-rated-movies", Name = nameof(GetTopRatedMovies))]
    public IActionResult GetTopRatedMovies(int minVotes = 1000)
    {

        var movies = _titledataservice
            .GetTopRatedMovies(0, 10, minVotes)
            .Select(CreateTitleModel);

        return Ok(movies);
    }

    /*
    [HttpGet]
    public IActionResult GetTitles(int userid)
    {
        var titles = _titledataservice
            .GetTitles()
            .Select(CreateTitleModel);
        return Ok(titles);
    }
    */

    [HttpGet("{id}", Name = nameof(GetTitle))]
    public IActionResult GetTitle(string id)
    {

        var title = _titledataservice.GetTitle(id);

        if (title == null)
        {
            return NotFound();
        }
        var model = CreateTitleModel(title);

        return Ok(model);

    }
    [HttpGet("top-rated-tvseries", Name = nameof(GetTopRatedTvSeries))]
    public IActionResult GetTopRatedTvSeries(int minVotes = 1000)
    {

        var movies = _titledataservice
            .GetTopRatedTvSeries(0, 10, minVotes)
            .Select(CreateTitleModel);

        return Ok(movies);
    }

    /*
    [HttpPost]
    public IActionResult CreateTitle(CreateTitleModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var title = _titledataservice.CreateTitle(user.Id, model.PrimaryTitle, model.OriginalTitle, model.IsAdult, model.StartYear);
            return CreatedAtRoute(nameof(GetTitle), new { id = title.Id }, CreateTitleModel(title));
        }
        catch
        { 
            return Unauthorized();
        }
    }
    */

    /*
    [HttpPut("{id}")]
    public IActionResult UpdateTitle(string id, CreateTitleModel model)
    {

        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var title = _titledataservice.GetTitle(id);

            if (title == null)
            {
                return NotFound();
            }

            title.PrimaryTitle = model.PrimaryTitle;
            title.OriginalTitle = model.OriginalTitle;
            title.IsAdult = model.IsAdult;
            title.StartYear = model.StartYear;

            _titledataservice.UpdateTitle(user.Id, title);

            return Ok(CreateTitleModel(title));
        }
        catch
        {
            return Unauthorized();
        }
    }

    */

    /*
    [HttpDelete("{id}")]
    public IActionResult DeleteTitle(string id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            var result = _titledataservice.DeleteTitle(user.Id, id);

            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
        catch 
        { 
            return Unauthorized(); 
        }
    }

    */

    private TitleModel? CreateTitleModel(Title? title)
    {
        if (title == null)
        {
            return null;
        }

        var model = title.Adapt<TitleModel>();
        model.Url = GetUrl(title.Id);

        return model;
    }

    private AllTitlesModel? CreateAllTitlesModel(Title? title)
    {
        if (title == null)
        {
            return null;
        }

        var model = title.Adapt<AllTitlesModel>();
        model.Url = GetUrl(title.Id);

        return model;
    }


    private string? GetUrl(string id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetTitle), new { id });
    }

    private string? GetLink(string linkName, int page, int pageSize)
    {
        return _linkGenerator.GetUriByName(
                    HttpContext,
                    linkName,
                    new { page, pageSize }
                    );
    }
}
