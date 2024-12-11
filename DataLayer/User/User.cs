namespace DataLayer;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public DateTime Birthday { get; set; }
    public string Phonenumber { get; set; } 

    public IList<UserRating> UserRating { get; set; }
    public IList<BookmarkName> BookmarkName { get; set;}
    public IList<BookmarkTitle> BookmarkTitle { get; set; }
    public IList<Searches> Searches { get; set; }
}
