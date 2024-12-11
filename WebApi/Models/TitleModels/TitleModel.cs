using DataLayer;
using WebApi.Models.KnownForTitlesModels;
using WebApi.Models.PlotAndPosterModels;
using WebApi.Models.RatingModels;
using WebApi.Models.TitleGenreModels;

namespace WebApi.Models.TitleModels
{
    public class TitleModel
    {
        public string? Url { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; } = string.Empty;
        public string OriginalTitle { get; set; } = string.Empty;
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
        public PlotAndPosterModel PlotAndPoster { get; set; }
        public TitleRatingModel TitleRating { get; set; }

        public IList<TitleGenreModel> TitleGenre { get; set; }
        public IList<KnownForTitlesModelName> KnownForTitles { get; set; }

    }
}
