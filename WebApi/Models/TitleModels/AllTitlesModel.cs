using WebApi.Models.PlotAndPosterModels;
using WebApi.Models.TitleGenreModels;

namespace WebApi.Models.TitleModels
{
    public class AllTitlesModel
    {
        public string? Url { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; } = string.Empty;
        public TitleGenreModel TitleGenre { get; set; }
        public PlotAndPosterModel PlotAndPoster { get; set; }
    }
}
