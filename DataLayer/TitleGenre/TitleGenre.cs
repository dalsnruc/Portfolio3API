namespace DataLayer;

public class TitleGenre
{
    public string TitleId { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; } 

    public Title Title { get; set; }

}
