using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.SearchesModels;

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
    public IActionResult GetSearches(int page = 0, int pageSize = 10)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
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
    public IActionResult GetSearch(int id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
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
    public IActionResult CreateSearch(CreateSearchesModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            _searchesdataservice.SaveSearch(user.Id, model.Content);

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
    public IActionResult DeleteSearch(int id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
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
