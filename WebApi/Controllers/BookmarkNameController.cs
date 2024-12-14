using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.BookmarkNameModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/bookmarknames")]

public class BookmarkNameController : BaseController
{
    IBookmarkNameDataService _bookmarknamedataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public BookmarkNameController(
        IBookmarkNameDataService bookmarkNameDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _bookmarknamedataservice = bookmarkNameDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;
    }

    
    [HttpGet(Name = nameof(GetBookmarkNames))]
    [Authorize]
    public IActionResult GetBookmarkNames(int page = 0, int pageSize = 20)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var bookmarknames = _bookmarknamedataservice
            .GetBookmarkNames(user.Id, page, pageSize)
            .Select(CreateBookmarkNameModel);
            var numberOfItems = _bookmarknamedataservice.NumberOfBookmarkNames(user.Id);
            object result = CreatePaging(
                nameof(GetBookmarkNames),
                page,
                pageSize,
                numberOfItems,
                bookmarknames);
            return Ok(result);
        }
        catch
        {
            return Unauthorized();
        }
    }
    

    [HttpGet("{id}", Name = nameof(GetBookmarkName))]
    public IActionResult GetBookmarkName(string id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var bookmarkname = _bookmarknamedataservice.GetBookmarkName(user.Id, id);

            if (bookmarkname == null)
            {
                return NotFound();
            }
            var model = CreateBookmarkNameModel(bookmarkname);

            return Ok(model);
        }
        catch
        { 
            return Unauthorized();
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateBookmarkName(CreateBookmarkNameModel model)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var bookmarkname = _bookmarknamedataservice.CreateBookmarkName(user.Id,model.NameId);

            return Created("", new CreateBookmarkNameModel
            {
                UserId = user.Id,
                NameId = bookmarkname.NameId
            });
        }
        catch
        {
            return Unauthorized();
        }
    }


    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteBookmarkName(string id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var result = _bookmarknamedataservice.DeleteBookmarkName(user.Id, id);

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


    private BookmarkNameModel? CreateBookmarkNameModel(BookmarkName? bookmarkname)
    {
        if (bookmarkname == null)
        {
            return null;
        }

        var model = bookmarkname.Adapt<BookmarkNameModel>();
        model.Url = GetUrl(bookmarkname.NameId);

        return model;
    }


    private string? GetUrl(string id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetBookmarkName), new { id });
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
