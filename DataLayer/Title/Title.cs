using System.ComponentModel.DataAnnotations;

namespace DataLayer;

public class Title
{

    [Key]
    public string Id { get; set; }
    public string TitleType { get; set; }
    public string PrimaryTitle { get; set; }
    public string OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public string? StartYear { get; set; }
    public PlotAndPoster PlotAndPoster { get; set; }

    public IList<TitleGenre> TitleGenre { get; set; }

    public TitleRating TitleRating { get; set; }
    public IList <KnownForTitles> KnownForTitles { get; set; }



}
