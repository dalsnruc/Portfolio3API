
namespace DataLayer;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<TitleGenre> TitleGenres { get; set; }

}
