namespace DataLayer;

public interface IUserDataService
{

    IList<User> GetUsers();
    IList<User> GetUsers(int userid, int page, int pageSize);
    User? GetUser(string username);
    User? GetUser(int userid, int id);

    User CreateUser(string username, string password, string email, DateTime birthday, string phonenumber);
    bool UpdateUser(string username, string password, string email, DateTime birthday, string phonenumber);
    bool UpdateUser(int userid, User user);
    bool DeleteUser(int userid, string username);

    int NumberOfUsers();
    


}
