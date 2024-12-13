namespace DataLayer;

public interface IUserDataService
{

    IList<User> GetUsers();
    IList<User> GetUsers(int userid, int page, int pageSize);
    User? GetUser(string username);
    User? GetUser(int userid, int id);

    User CreateUser(string username, string password, string email, DateTime birthday, string phonenumber, string salt);
    bool UpdateUser(string username, string email, DateTime birthday, string phonenumber);
    bool UpdateUser(string username, User user);
    bool DeleteUser(int userid, string username);

    int NumberOfUsers();
    


}
