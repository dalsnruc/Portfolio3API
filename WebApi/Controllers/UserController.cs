using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using WebApi.Models.TitleModels;
using WebApi.Models.UserModels;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]

public class UserController : BaseController
{
    IUserDataService _userdataservice;
    private readonly IConfiguration _configuration;
    private readonly LinkGenerator _linkGenerator;
    private readonly Hashing _hashing;

    public UserController(
        IUserDataService userDataService,
        IConfiguration configuration,
        Hashing hashing,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _userdataservice = userDataService;
        _configuration = configuration;
        _linkGenerator = linkGenerator;
        _hashing = hashing;
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

        if(_userdataservice.GetUser(model.Username)!= null)
        {
            return BadRequest();
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            return BadRequest();
        }

        (var hashedPwd, var salt) = _hashing.Hash(model.Password);

        var user = _userdataservice.CreateUser(model.Username, hashedPwd, model.Email, model.Birthday, model.Phonenumber, salt);
        //return CreatedAtRoute(nameof(GetUser), new { id = user.Id }, CreateUserModel(user));
        return Ok();


    }
    
    [HttpPut("login", Name = nameof(Login))]
    public IActionResult Login(LogInUserModel model)
    {
        var user = _userdataservice.GetUser(model.Username);

        if(user == null)
        {
            return BadRequest();
        }

        if(!_hashing.Verify(model.Password, user.Password, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var secret = _configuration.GetSection("Auth:Secret").Value;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new {username = user.Username, token = jwt});
    }
    
    [HttpPut("update", Name = nameof(UpdateUser))]
    public IActionResult UpdateUser(UpdateUserModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.Birthday = model.Birthday;
            user.Phonenumber = model.Phonenumber;

            bool updated = _userdataservice.UpdateUser(
                user.Username,
                user.Email,
                user.Birthday,
                user.Phonenumber
            );

            if (updated)
            {
                return Ok(CreateUserModel(user));
            }

            return BadRequest("Error");
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
