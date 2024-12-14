using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.UserRatingModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/userratings")]

public class UserRatingController : BaseController
{
    IUserRatingDataService _userratingdataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public UserRatingController(
        IUserRatingDataService userRatingDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _userratingdataservice = userRatingDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;
    }


    [HttpGet(Name = nameof(GetUserRatings))]
    [Authorize]
    public IActionResult GetUserRatings(int page = 0, int pageSize = 20)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var userratings = _userratingdataservice
            .GetUserRatings(user.Id, page, pageSize)
            .Select(CreateUserRatingModel);
            var numberOfItems = _userratingdataservice.NumberOfUserRatings(user.Id);
            object result = CreatePaging(
                nameof(GetUserRatings),
                page,
                pageSize,
                numberOfItems,
                userratings);
            return Ok(result);
        }
        catch
        {
            return Unauthorized();
        }
    }


    [HttpGet("{id}", Name = nameof(GetUserRating))]
    [Authorize]
    public IActionResult GetUserRating(string id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var userrating = _userratingdataservice.GetUserRating(user.Id, id);

            if (userrating == null)
            {
                return NotFound();
            }
            var model = CreateUserRatingModel(userrating);

            return Ok(model);
        }
        catch
        {
            return Unauthorized();
        }
    }


    [HttpPost]
    [Authorize]
    public IActionResult CreateUserRating(CreateUserRatingModel model)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);
            var userrating = _userratingdataservice.CreateUserRating(user.Id, model.TitleId, model.Rating);

            return Created("", new CreateUserRatingModel
            {
                UserId = user.Id,
                TitleId = userrating.TitleId,
                Rating = userrating.Rating
            });
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPut("{titleid}")]
    [Authorize]
    public IActionResult UpdateUserRating(string titleid, UpdateUserRatingModel model)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var userrating = _userratingdataservice.GetUserRating(user.Id, titleid);

            if (userrating == null)
            {
                return NotFound();
            }

            userrating.TitleId = titleid;
            userrating.Rating = model.Rating;
            userrating.Date = DateTime.UtcNow;

            _userratingdataservice.UpdateUserRating(user.Id, userrating);

            return Ok(CreateUserRatingModel(userrating));
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteUserRating(string id)
    {
        try
        {
            var username = User.Identity?.Name;
            var user = _userdataservice.GetUser(username);

            var result = _userratingdataservice.DeleteUserRating(user.Id, id);

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

    


    private UserRatingModel? CreateUserRatingModel(UserRating? userrating)
    {
        if (userrating == null)
        {
            return null;
        }

        var model = userrating.Adapt<UserRatingModel>();
        model.Url = GetUrl(userrating.TitleId);

        return model;
    }


    private string? GetUrl(string id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetUserRating), new { id });
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
