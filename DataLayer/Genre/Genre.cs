
namespace DataLayer;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<TitleGenre> TitleGenres { get; set; }

}
