using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.NameModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/names")]

public class NameController : BaseController
{
    INameDataService _namedataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public NameController(
        INameDataService nameDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _namedataservice = nameDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet(Name = nameof(GetNamesPaged))]
    public IActionResult GetNamesPaged(int page = 0, int pageSize = 10)
    {

        var names = _namedataservice
        .GetNames(page, pageSize)
        .Select(CreateAllNamesModel);
        var numberOfItems = _namedataservice.NumberOfNames();
        object result = CreatePaging(
            nameof(GetNamesPaged),
            page,
            pageSize,
            numberOfItems,
            names);
        return Ok(result);

    }


    [HttpGet("{id}", Name = nameof(GetName))]
    public IActionResult GetName(string id)
    {

        var name = _namedataservice.GetName(id);

        if (name == null)
        {
            return NotFound();
        }
        var model = CreateNameModel(name);

        return Ok(model);

    }

    /*
    [HttpPost]
    public IActionResult CreateName(CreateNameModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var name = _namedataservice.CreateName(user.Id, model.PrimaryName, model.BirthYear, model.DeathYear);
            return CreatedAtRoute(nameof(GetName), new { id = name.NameId }, CreateNameModel(name));
        }
        catch
        {
            return Unauthorized();
        }
    }
    */
    /*
    [HttpPut("{id}")]
    public IActionResult UpdateName(string id, CreateNameModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            var name = _namedataservice.GetName(user.Id, id);

            if (name == null)
            {
                return NotFound();
            }

            name.PrimaryName = model.PrimaryName;
            name.BirthYear = model.BirthYear;

            _namedataservice.UpdateName(user.Id, name);

            return Ok(CreateNameModel(name));
        }
        catch
        {
            return Unauthorized();
        }
    }
    */

    /*

    [HttpDelete("{id}")]
    public IActionResult DeleteName(string id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            var result = _namedataservice.DeleteName(user.Id, id);

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



    private NameModel? CreateNameModel(Name? name)
    {
        if (name == null)
        {
            return null;
        }

        var model = name.Adapt<NameModel>();
        model.Url = GetUrl(name.NameId);

        return model;
    }

    private AllNamesModel? CreateAllNamesModel(Name? name)
    {
        if (name == null)
        {
            return null;
        }

        var model = name.Adapt<AllNamesModel>();
        model.Url = GetUrl(name.NameId);

        return model;
    }

    private string? GetUrl(string id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetName), new { id });
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
