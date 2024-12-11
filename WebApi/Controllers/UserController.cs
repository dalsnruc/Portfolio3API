using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApi.Models.TitleModels;
using WebApi.Models.UserModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]

public class UserController : BaseController
{
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public UserController(
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userdataservice
            .GetUsers()
            .Select(CreateUserModel);
        return Ok(users);
    }

    [HttpGet("paged", Name = nameof(GetUsersPaged))]
    public IActionResult GetUsersPaged(int page = 0, int pageSize = 2)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            var users = _userdataservice
            .GetUsers(user.Id, page, pageSize)
            .Select(CreateAllUsersModel);
            var numberOfItems = _userdataservice.NumberOfUsers();
            object result = CreatePaging(
                nameof(GetUsers),
                page,
                pageSize,
                numberOfItems,
                users);
            return Ok(result);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpGet("{id}", Name = nameof(GetUser))]
    public IActionResult GetUser(int id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var getuser = _userdataservice.GetUser(user.Id, id);

            if (getuser == null)
            {
                return NotFound();
            }
            var model = CreateUserModel(getuser);

            return Ok(model);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var userL = _userdataservice.GetUser(username);
            var user = _userdataservice.CreateUser(userL.Id, model.Username, model.Password, model.Email, model.Birthday, model.Phonenumber);
            return CreatedAtRoute(nameof(GetUser), new { id = user.Id }, CreateUserModel(user));
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, CreateUserModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var userL = _userdataservice.GetUser(username);

            var user = _userdataservice.GetUser(userL.Id, id);

            if (user == null)
            {
                return NotFound();
            }

            user.Username = model.Username;
            user.Password = model.Password;
            user.Email = model.Email;
            user.Birthday = model.Birthday;
            user.Phonenumber = model.Phonenumber;

            _userdataservice.UpdateUser(userL.Id, user);

            return Ok(CreateUserModel(user));
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpDelete("{username}")]
    public IActionResult DeleteUser(string username)
    {
        try
        {
            var usernameL = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var userL = _userdataservice.GetUser(usernameL);

            var result = _userdataservice.DeleteUser(userL.Id, username);

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

    private UserModel? CreateUserModel(User? user)
    {
        if (user == null)
        {
            return null;
        }

        var model = user.Adapt<UserModel>();
        model.Url = GetUrl(user.Id);

        return model;
    }

    private AllUsersModel? CreateAllUsersModel(User? user)
    {
        if (user == null)
        {
            return null;
        }

        var model = user.Adapt<AllUsersModel>();
        model.Url = GetUrl(user.Id);

        return model;
    }

    private string? GetUrl(int id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetUser), new { id });
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
