using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.BookmarkTitleModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/bookmarktitles")]

public class BookmarkTitleController : BaseController
{
    IBookmarkTitleDataService _bookmarktitledataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public BookmarkTitleController(
        IBookmarkTitleDataService bookmarkTitleDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _bookmarktitledataservice = bookmarkTitleDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;
    }


    [HttpGet(Name = nameof(GetBookmarkTitles))]
    [Authorize]
    public IActionResult GetBookmarkTitles(int page = 0, int pageSize = 2)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var bookmarktitles = _bookmarktitledataservice
            .GetBookmarkTitles(user.Id, page, pageSize)
            .Select(CreateBookmarkTitleModel);
            var numberOfItems = _bookmarktitledataservice.NumberOfBookmarkTitles(user.Id);
            object result = CreatePaging(
                nameof(GetBookmarkTitles),
                page,
                pageSize,
                numberOfItems,
                bookmarktitles);
            return Ok(result);
        }
        catch
        {
            return Unauthorized();
        }
    }


    [HttpGet("{id}", Name = nameof(GetBookmarkTitle))]
    [Authorize]
    public IActionResult GetBookmarkTitle(string id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var bookmarktitle = _bookmarktitledataservice.GetBookmarkTitle(user.Id, id);

            if (bookmarktitle == null)
            {
                return NotFound();
            }
            var model = CreateBookmarkTitleModel(bookmarktitle);

            return Ok(model);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateBookmarkTitle(CreateBookmarkTitleModel model)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var bookmarktitle = _bookmarktitledataservice.CreateBookmarkTitle(user.Id, model.TitleId);

            return Created("", new CreateBookmarkTitleModel
            {
                UserId = user.Id,
                TitleId = bookmarktitle.TitleId
            });
        }
        catch
        {
            return Unauthorized();
        }
    }


    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteBookmarkTitle(string id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var result = _bookmarktitledataservice.DeleteBookmarkTitle(user.Id, id);

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


    private BookmarkTitleModel? CreateBookmarkTitleModel(BookmarkTitle? bookmarktitle)
    {
        if (bookmarktitle == null)
        {
            return null;
        }

        var model = bookmarktitle.Adapt<BookmarkTitleModel>();
        model.Url = GetUrl(bookmarktitle.TitleId);

        return model;
    }


    private string? GetUrl(string id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetBookmarkTitle), new { id });
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
