using System.ComponentModel.DataAnnotations;

namespace DataLayer;

public class PlotAndPoster
{
    [Key]
    public string TitleId { get; set; }
    public string Plot { get; set; }
    public string Poster { get; set; }

}
