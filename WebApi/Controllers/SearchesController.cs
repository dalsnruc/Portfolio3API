using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Models.NameModels;
using WebApi.Models.SearchesModels;
using WebApi.Models.TitleModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/searches")]

public class SearchesController : BaseController
{
    ISearchesDataService _searchesdataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public SearchesController(
        ISearchesDataService searchesDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _searchesdataservice = searchesDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;
    }


    [HttpGet(Name = nameof(GetSearches))]
    [Authorize]
    public IActionResult GetSearches(int page = 0, int pageSize = 10)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var searches = _searchesdataservice
            .GetSearches(user.Id, page, pageSize)
            .Select(CreateSearchesModel);
            var numberOfItems = _searchesdataservice.NumberOfSearches(user.Id);
            object result = CreatePaging(
                nameof(GetSearches),
                page,
                pageSize,
                numberOfItems,
                searches);
            return Ok(result);
        }
        catch
        {
            return Unauthorized();
        }
    }


    [HttpGet("{id}", Name = nameof(GetSearch))]
    [Authorize]
    public IActionResult GetSearch(int id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var search = _searchesdataservice.GetSearch(user.Id, id);

            if (search == null)
            {
                return NotFound();
            }
            var model = CreateSearchesModel(search);

            return Ok(model);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateSearch(CreateSearchesModel model)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            _searchesdataservice.SaveSearchAsync(user.Id, model.Content);

            return Created("", new CreateSearchesModel
            {
                UserId = user.Id,
                Content = model.Content
            });
        }
        catch
        {
            return Unauthorized();
        }
    }




    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteSearch(int id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var result = _searchesdataservice.DeleteSearch(user.Id, id);

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

    //Controller when a user uses the searchbar and searches for something
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] bool exactMatch = false)
    {

        //Check user status:
        var userIdClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //If the user is not logged in, the userID will be set as a null value
        int? userId = null;

        if (!string.IsNullOrEmpty(userIdClaim))
        {
            userId = int.Parse(userIdClaim);
        }

        //Check the searchTerm (string)
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("The Search cannot be empty!");

        //Determine if the search is more likely a Title or a Name
        var isTitleSearch = IsTitleSearch(searchTerm);
        var isNameSearch = IsNameSearch(searchTerm);

        //Save the "search string given", whether or not a user is logged in
        // - and wether or not the search was succesfull 
        await _searchesdataservice.SaveSearchAsync(userId, searchTerm);

        if (isTitleSearch && isNameSearch)
        {
            //If the search matches both, the return both
            var titleResults = await _searchesdataservice.SearchTitlesAsync(searchTerm, exactMatch);
            var nameResults = await _searchesdataservice.SearchNamesAsync(searchTerm, exactMatch);
            return Ok(new { Titles = titleResults, Names = nameResults });
        }
        else if (isTitleSearch)
        {
            var titleResults = await _searchesdataservice.SearchTitlesAsync(searchTerm, exactMatch);
            return Ok(titleResults.Select(t => new TitleModel
            {
                // We might need url her?!
                TitleType = t.TitleType,
                PrimaryTitle = t.PrimaryTitle,
                OriginalTitle = t.OriginalTitle,
                IsAdult = t.IsAdult,
                StartYear = t.StartYear,
                /*
                PlotAndPoster = t.PlotAndPoster,
                TitleRating = t.TitleRating,
                TitleGenre = t.TitleGenre,
                */

            }));
        }
        else if (isNameSearch)
        {
            var nameResults = await _searchesdataservice.SearchNamesAsync(searchTerm, exactMatch);
            return Ok(nameResults.Select(n => new NameModel
            {
                // We might need url her?!
                NameId = n.NameId,
                PrimaryName = n.PrimaryName,
                BirthYear = n.BirthYear,
                DeathYear = n.DeathYear,

                AverageRating = n.AverageRating,
                /*
                KnownForTitles = n.KnownForTitles,
                NameProfession = n.NameProfession,
                */
            }));
        }
        else
        {
            return NotFound("Could not find what you were looking for.");
        }

    }

    // Helper method to determine wether searh term is more like a title or name
    private bool IsTitleSearch(string searchTerm)
    {
        //If the search contains spaces (more than one word) it is most likely to be a movie or a tv-show
        return !searchTerm.Contains(" ");
    }

    private bool IsNameSearch(string searchTerm)
    {
        //If the search does NOT contain spaces (only 1 word) it is most likely a name of an actor or director
        return searchTerm.Contains(" ");
    }

    private SearchesModel? CreateSearchesModel(Searches? searches)
    {
        if (searches == null)
        {
            return null;
        }

        var model = searches.Adapt<SearchesModel>();
        model.Url = GetUrl(searches.SearchId);

        return model;
    }


    private string? GetUrl(int id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetSearches), new { id });
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