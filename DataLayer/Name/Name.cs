namespace DataLayer;

public class Name
{
    public string NameId {  get; set; }
    public string PrimaryName { get; set; }
    public string BirthYear { get; set; }
    public string? DeathYear { get; set; }
    public float? AverageRating { get; set; }
    public IList <KnownForTitles> KnownForTitles { get; set; }

    public IList <NameProfession> NameProfession { get; set; }

    public IList <BookmarkName> BookmarkNames { get; set; }
}
