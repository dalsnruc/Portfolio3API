
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DataLayer;

public class UserDataService : IUserDataService
{
    public IList<User> GetUsers()
    {
        var db = new imdbContext();
        return db.Users.ToList();

    }

    public IList<User> GetUsers(int userid, int page, int pageSize)
    {
        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        return db.Users
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public User? GetUser(string username)
    {
        var db = new imdbContext();
        return db.Users.FirstOrDefault(u => u.Username == username);
    }

    public User? GetUser(int userid, int id)
    {

        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        return db.Users
            .Include(u => u.UserRating)
            .ThenInclude(ur => ur.Title)
            .Include(u => u.BookmarkName)
            .ThenInclude(bn => bn.Name)
            .Include(u => u.BookmarkTitle)
            .ThenInclude(bt => bt.Title)
            .Include(u => u.Searches)
            .FirstOrDefault(u => u.Id == id);
    }

    public User CreateUser(string username, string password, string email, DateTime birthday, string phonenumber, string salt)
    {
        var db = new imdbContext();

        int id = db.Users.Max(x => x.Id) + 1;

        
        DateTime utcBirthday = birthday.Kind == DateTimeKind.Utc
            ? birthday
            : birthday.ToUniversalTime();

        var user = new User
        {
            Id = id,
            Username = username,
            Password = password,
            Email = email,
            Birthday = utcBirthday,
            Phonenumber = phonenumber,
            Salt = salt
        };

        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }

    public bool UpdateUser(string loggedinusername, string email, DateTime birthday, string phonenumber)
    {
        var db = new imdbContext();
        var user = db.Users.FirstOrDefault(u => u.Username == loggedinusername);

       
        DateTime utcBirthday = birthday.Kind == DateTimeKind.Utc
            ? birthday
            : birthday.ToUniversalTime();

        if (user == null)
        {
            return false;
        }

        user.Email = email;
        user.Birthday = utcBirthday;
        user.Phonenumber = phonenumber;

        return db.SaveChanges() > 0;
    }

    public bool UpdateUser(string username, User user)
    {
        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Username == username) == null)
        {
            throw new ArgumentException("User not found");
        }

        db.Update(user);

        return db.SaveChanges() > 0;

    }


    public bool DeleteUser(int userid, string username)
    {
        var db = new imdbContext();

        if (db.Users.FirstOrDefault(x => x.Id == userid) == null)
        {
            throw new ArgumentException("User not found");
        }

        var user = db.Users.FirstOrDefault(u => u.Username == username);

        if(user == null)
        {
            return false;
        }

        db.Users.Remove(user);
        return db.SaveChanges() > 0;


    }


    public int NumberOfUsers()
    {
        var db = new imdbContext();
        return db.Users.Count();
    }
}
